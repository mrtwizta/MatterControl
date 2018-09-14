﻿using System;
using System.Globalization;
using System.IO;
using System.Threading;
using MatterHackers.Agg.Platform;
using MatterHackers.Agg.UI;
using MatterHackers.MatterControl.DataStorage;
using MatterHackers.MatterControl.SettingsManagement;
using MatterHackers.MatterControl.SlicerConfiguration;
using Microsoft.Extensions.Configuration;
//using Mindscape.Raygun4Net;

namespace MatterHackers.MatterControl
{
	public static class MatterControlLib
	{
		private const int RaygunMaxNotifications = 15;

		private static int raygunNotificationCount = 0;

		//private static RaygunClient _raygunClient;

		private class SlicerOptions
		{
			public bool Debug { get; set; }
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Main()
		{
			// this sets the global culture for the app and all new threads
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

			// and make sure the app is set correctly
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

			//if (AggContext.OperatingSystem == OSType.Mac)
			//{
			//	_raygunClient = new RaygunClient("qmMBpKy3OSTJj83+tkO7BQ=="); // this is the Mac key
			//}
			//else
			//{
			//	_raygunClient = new RaygunClient("hQIlyUUZRGPyXVXbI6l1dA=="); // this is the PC key
			//}

			// Set default Agg providers
			//AggContext.Config.ProviderTypes.SystemWindow = "MonoMacGameView.MyOpenGLView, MonoMacGameView";
			AggContext.Config.ProviderTypes.SystemWindowProvider = "MatterHackers.Agg.UI.MacWindowProvider, MatterControlMac";
            AggContext.Config.ProviderTypes.OsInformationProvider = "MonoMacGameView.MacInformationProvider, MatterControlMac";

            AggContext.Config.ProviderTypes.StaticDataProvider = "MatterHackers.Agg.MacStaticData, MatterControlMac";
            AggContext.Config.ProviderTypes.ImageIOProvider = "MatterHackers.Agg.Image.MacImageIO, MatterControlMac";
            AggContext.Config.ProviderTypes.DialogProvider = "MatterHackers.Agg.Platform.MacFileDialogProvider, MatterControlMac";

            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

			// Load optional user configuration
			//IConfiguration config = new ConfigurationBuilder()
   //             .AddJsonFile("appsettings.json", optional: true,reloadOnChange: false)
			//	//.AddJsonFile(Path.Combine(userProfilePath, "MatterControl.json"), optional: true)
			//	.Build();

			//// Override defaults via configuration
			//config.Bind("Agg:ProviderTypes", AggContext.Config.ProviderTypes);
			//config.Bind("Agg:GraphicsMode", AggContext.Config.GraphicsMode);

			//Slicer.RunInProcess = config.GetValue<bool>("MatterControl:Slicer:Debug");

			// Make sure we have the right working directory as we assume everything relative to the executable.
			Directory.SetCurrentDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));




			Datastore.Instance.Initialize();

            //#if !DEBUG
            //			// Conditionally spin up error reporting if not on the Stable channel
            //			string channel = UserSettings.Instance.get(UserSettingsKey.UpdateFeedType);
            //			if (string.IsNullOrEmpty(channel) || channel != "release" || OemSettings.Instance.WindowTitleExtra == "Experimental")
            //#endif
            //			{
            //				System.Windows.Forms.Application.ThreadException += (s, e) =>
            //				{
            //					if(raygunNotificationCount++ < RaygunMaxNotifications)
            //					{
            //						_raygunClient.Send(e.Exception);
            //					}
            //				};

            //				AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            //				{
            //					if (raygunNotificationCount++ < RaygunMaxNotifications)
            //					{
            //						_raygunClient.Send(e.ExceptionObject as Exception);
            //					}
            //				};
            //			}

            // Init platformFeaturesProvider before ShowAsSystemWindow
            string platformFeaturesProvider = "MatterHackers.MatterControl.MacPlatformsFeatures, MatterControlMac";
			AppContext.Platform = AggContext.CreateInstanceFrom<INativePlatformFeatures>(platformFeaturesProvider);
			AppContext.Platform.ProcessCommandline();

			// Get startup bounds from MatterControl and construct system window
			//var systemWindow = new DesktopMainWindow(400, 200)
			var (width, height) = RootSystemWindow.GetStartupBounds();

			var systemWindow = Application.LoadRootWindow(width, height);
            systemWindow.DebugShowBounds = false;
			systemWindow.ShowAsSystemWindow();
		}

		// ** Standard Winforms Main ** //
		//[STAThread]
		//static void Main()
		//{
		//	Application.EnableVisualStyles();
		//	Application.SetCompatibleTextRenderingDefault(false);
		//	Application.Run(new Form1());
		//}
	}
}
