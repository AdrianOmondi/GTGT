#if UNITY_EDITOR && UNITY_IOS
using UnityEngine;
using ActualUnityEditor = UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google;

namespace SkillzSDK.Internal.Build.iOS
{
	using ActualUnityEditor.SKZXCodeEditor;
	using SkillzSDK.Settings;

	//Disable warnings about code blocks that will never be reached.
#pragma warning disable 162, 429

	public static class SkillzPostProcessBuild
	{
		//The following public static fields can be modified for developers that want to automate their Unity builds.
		//Otherwise, some dialogs will appear at the end of every new/replace build.

		/// <summary>
		/// If this is true, then this class's static fields will be used instead of prompting the developer at build time.
		/// <summary>
		private const bool AutoBuild_On = false;

		/// <summary>
		/// Whether this is a portrait game. Only used if "AutoBuild_Use" is true.
		/// </summary>
		private const bool AutoBuild_IsPortrait = false;
		/// <summary>
		/// The full path of the "SkillzSDK-iOS.embeddedframework" folder that came with the downloaded Skillz SDK.
		/// Only used if "AutoBuild_Use" is true.
		/// </summary>
		private const string AutoBuild_SkillzPath = "/Users/myUsername/Downloads/sdk_ios_10.1.19/Skillz.framework";

		/// <summary>
		/// A file with this name is used to track whether a build is appending or replacing.
		/// </summary>
		private const string checkAppendFileName = ".skillzTouch";

		[ActualUnityEditor.Callbacks.PostProcessBuildAttribute(45)] // fires right before pod install
		public static void PostProcessBuild_iOS(ActualUnityEditor.BuildTarget build, string path)
		{
			if (build == ActualUnityEditor.BuildTarget.iOS)
			{
				var fileName = path + "/Podfile";
				List<string> txtLines = File.ReadAllLines(fileName).ToList();

				var mainTargetLine = "target 'Unity-iPhone' do";
				var scriptLine1 = "  script_phase :name => 'Skillz Postprocess', :script => 'if [ -e \"${BUILT_PRODUCTS_DIR}/${FRAMEWORKS_FOLDER_PATH}/Skillz.framework/postprocess.sh\" ]; then";
				var scriptLine2 = "    /bin/sh \"${BUILT_PRODUCTS_DIR}/${FRAMEWORKS_FOLDER_PATH}/Skillz.framework/postprocess.sh\"";
				var scriptLine3 = "  fi'";

				txtLines.Insert(txtLines.IndexOf(mainTargetLine) + 1, scriptLine3); // insert lines backwards
				txtLines.Insert(txtLines.IndexOf(mainTargetLine) + 1, scriptLine2);
				txtLines.Insert(txtLines.IndexOf(mainTargetLine) + 1, scriptLine1);

				txtLines.Add("post_install do |installer|");
				txtLines.Add("  installer.pods_project.build_configurations.each do |config|");
				txtLines.Add("    config.build_settings['EXCLUDED_ARCHS[sdk=iphonesimulator*]'] = 'arm64'");
				txtLines.Add("    config.build_settings['ARCHS'] = 'arm64'");
				txtLines.Add("    config.build_settings['ENABLE_BITCODE'] = 'NO'");
				txtLines.Add("  end");
				txtLines.Add("end");

				txtLines.Add("# This podfile has been modified by the Skillz export");

				File.WriteAllLines(fileName, txtLines);
			}

		}

		[ActualUnityEditor.Callbacks.PostProcessBuild(9090)]
		public static void OnPostProcessBuild(ActualUnityEditor.BuildTarget build, string path)
		{
			//Make sure this build is for iOS.
			//Unity 4 uses 'iPhone' for the enum value; Unity 5 changes it to 'iOS'.
			if (build.ToString() != "iPhone" && build.ToString() != "iOS")
			{
				UnityEngine.Debug.LogWarning("Skillz cannot be set up for a platform other than iOS.");
				return;
			}
			if (Application.platform != RuntimePlatform.OSXEditor)
			{
				UnityEngine.Debug.LogError("Skillz cannot be set up for XCode automatically on a platform other than OSX.");
				return;
			}

			//Get whether this is an append build by checking whether a custom file has already been created.
			//If it is, then nothing needs to be modified.
			string checkAppendFilePath = Path.Combine(path, checkAppendFileName);
			FileInfo checkAppend = new FileInfo(checkAppendFilePath);
			if (checkAppend.Exists)
			{
				return;
			}

			checkAppend.Create().Close();

			//Set up XCode project settings.
			var xcodeProjectPath = Path.Combine(path, "Unity-iPhone.xcodeproj/project.pbxproj");
			Debug.Log($"Loading the XCode project at '{xcodeProjectPath}'");

			try
			{
				using (var xcodeProjectSettings = XcodeProjectSettings.Load(xcodeProjectPath))
				{
					xcodeProjectSettings.DisableBitcode();
					xcodeProjectSettings.ModifyMiscellaneous();
					xcodeProjectSettings.AddFrameworks();
				}
			}
			catch (System.Exception)
			{
				UnityEngine.Debug.LogError("Skillz automated XCode editing failed!");
			}
#if UNITY_2019_3_OR_NEWER
			ActualUnityEditor.iOS.Xcode.PBXProject pbProject = new ActualUnityEditor.iOS.Xcode.PBXProject();
			pbProject.ReadFromString(File.ReadAllText(xcodeProjectPath));
			if (pbProject != null)
			{
				// We need to add the UnityFramework.framework to the "Link Binary with Libraries" step for the main target.
				// Without this, Skillz won't be able to find the game's app delegate
				string mainTarget = pbProject.GetUnityMainTargetGuid();
				pbProject.AddFrameworkToProject(mainTarget, "UnityFramework.framework", false);
			}
#endif // UNITY_2019_3_OR_NEWER
			XCProject project = new XCProject(path);
			if (project != null)
			{
				//Unity_4 doesn't exist so we check for Unity 5 defines.  Unity 6 is used for futureproofing.
#if !UNITY_5 && !UNITY_6
				project.AddFile(Path.Combine(Application.dataPath, "Skillz", "Internal", "Build", "iOS", "IncludeInXcode", "Skillz+Unity.mm"));
				SetAllowSkillzExit(Path.Combine(path, "Libraries", "Skillz", "Internal", "Build", "iOS", "IncludeInXcode", "Skillz+Unity.mm"));
				SetGameHasSyncBot(Path.Combine(path, "Libraries", "Skillz", "Internal", "Build", "iOS", "IncludeInXcode", "Skillz+Unity.mm"));
#endif // !UNITY_5 && !UNITY_6
                AddTimestamp(path);
				project.Save();
			}
			else
			{
				UnityEngine.Debug.LogError("Skillz automated XCode export failed!");
				return;
			}
		}

		private static void SetAllowSkillzExit(string skillzPlusUnityPath)
		{
			Debug.Log($"[Skillz] Setting allowExit at '{skillzPlusUnityPath}'");

			var allLines = File.ReadAllLines(skillzPlusUnityPath).ToList();
			var index = allLines.FindIndex(0, line => line.Contains("allowExit:YES"));
			if (index == -1)
			{
				Debug.LogWarning($"[Skillz] Could not find 'allowExit:YES'!");
				return;
			}

			var allowExit = SkillzSettings.Instance.AllowSkillzExit ? "YES" : "NO";
			allLines[index] = allLines[index].Replace("allowExit:YES", $"allowExit:{allowExit}");

			File.WriteAllLines(skillzPlusUnityPath, allLines);
		}

		private static void SetGameHasSyncBot(string skillzPlusUnityPath)
		{
			Debug.Log($"[Skillz] Setting game has sync bot at '{skillzPlusUnityPath}'");

			var allLines = File.ReadAllLines(skillzPlusUnityPath).ToList();
			var index = allLines.FindIndex(0, line => line.Contains("setGameHasSyncBot:NO"));
			if (index == -1)
			{
				Debug.LogWarning($"[Skillz] Could not find 'setGameHasSyncBot:NO'!");
				return;
			}

			var allowExit = SkillzSettings.Instance.HasSyncBot ? "YES" : "NO";
			allLines[index] = allLines[index].Replace("setGameHasSyncBot:NO", $"setGameHasSyncBot:{allowExit}");

			File.WriteAllLines(skillzPlusUnityPath, allLines);
		}

		private static bool SetUpSDKFiles(string projPath)
		{
			//Ask the user for the embeddedframework path if necessary.
			bool askForPath = true;
			string sdkPath = ".dummy";
			if (AutoBuild_On)
			{
				if (new DirectoryInfo(AutoBuild_SkillzPath).Exists)
				{
					askForPath = false;
					sdkPath = AutoBuild_SkillzPath;
				}
				else
				{
					ActualUnityEditor.EditorUtility.DisplayDialog("Skillz auto-build failed!",
					                            "Couldn't find the directory '" + AutoBuild_SkillzPath +
					                            	"'; please locate it manually in the following dialog.",
					                            "OK");
				}
			}

			var specialSdkPath = Path.Combine(Application.dataPath, "Plugins", "iOS", "Skillz.framework");
			if (System.IO.Directory.Exists(specialSdkPath))
			{
				// The iOS SDK was found at the 'special' path, so skip
				// prompting the user for its location.
				Debug.Log($"Adding Skillz.framework found at '{specialSdkPath}'");
				sdkPath = specialSdkPath;
			}
			else
			{
				while (askForPath && Path.GetFileName(sdkPath) != "Skillz.framework")
				{
					//If the user hit "cancel" on the dialog, quit out.
					if (sdkPath == "")
					{
						UnityEngine.Debug.Log("You canceled the auto-copying of the 'Skillz.framework'. " +
											"You must copy it yourself into '" + projPath + "' before building the XCode project.");
						return true;
					}

					sdkPath = ActualUnityEditor.EditorUtility.OpenFolderPanel("Select the Skillz.framework file",
															"", "");
				}
			}

			//Copy the SDK files into the XCode project.
			try
			{
				DirectoryInfo newDir = new DirectoryInfo(Path.Combine(projPath, "Skillz.framework"));
				if (newDir.Exists)
				{
					newDir.Delete();
					newDir.Create();
				}
				if (!CopyFolder(new DirectoryInfo(sdkPath), newDir))
				{
					newDir.Delete();
					throw new IOException("Couldn't copy the .framework contents");
				}
			}
			catch (System.Exception e)
			{
				PrintSDKFileError(e, sdkPath, projPath);
				return false;
			}

			return true;
		}
		private static bool CopyFolder(DirectoryInfo oldPath, DirectoryInfo newPath)
		{
			if (!oldPath.Exists)
			{
				return false;
			}
			if (!newPath.Exists)
			{
				newPath.Create();
			}

			//Copy each file.
			foreach (FileInfo oldFile in oldPath.GetFiles())
			{
				oldFile.CopyTo(Path.Combine(newPath.FullName, oldFile.Name), true);
			}
			//Copy each subdirectory.
			foreach (DirectoryInfo oldSubDir in oldPath.GetDirectories())
			{
				DirectoryInfo newSubDir = newPath.CreateSubdirectory(oldSubDir.Name);
				if (!CopyFolder(oldSubDir, newSubDir))
				{
					return false;
				}
			}

			return true;
		}
		private static void PrintSDKFileError(System.Exception e, string sdkPath, string projPath)
		{
			string manualInstructions = "Failed to copy the Skillz SDK files. Please manually copy '" + sdkPath +
										"' to '" + projPath + "/'. If this error persists, please contact " +
										"integrations@skillz.com.\n\nError: " + e.Message;
			ActualUnityEditor.EditorUtility.DisplayDialog("Skillz SDK setup failed!", manualInstructions, "OK");
		}
		private static void AddTimestamp(string parentFolderPath)
		{
			// Load the target file
			string filePath = parentFolderPath + "/MainApp/main.mm";
			string contents = File.ReadAllText(filePath);
			
			// Add timestamp to NSLog
			System.DateTime now = System.DateTime.Now;
			contents = contents.Replace("return 0;", "NSLog(@\"Build Time = " + now.ToString() + "\");\n\t\treturn 0;");

			// Replace existing file with new version
			File.Delete(filePath);
			File.WriteAllText(filePath, contents);
		}
	}

	//Restore the warnings that were disabled.
#pragma warning restore 162, 429
}
#endif
