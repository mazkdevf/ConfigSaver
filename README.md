## ConfigSaver

### What is ConfigSaver?,

##### ConfigSaver is Easy Framework to save username + password config, With Simple Encryption.

###### ConfigSaver is 100% Open-Source Too!

-------

#### What Nuget Packages does ConfigSaver use?

- ###### Newtonsoft.Json - 13.0.1

- ###### Eramake ECrypto - 1.1.6

--------------

##### To do List

- [ ] Better Encryption

- [ ] License / Key Saving

-------------

C-Sharp Console Example

```cs
using System;
using ConfigSaver;

namespace ConfigSaverTest
{
    class Program
    {
        static void Main(string[] args)
        {

            /// Read Config Saving Path
            CF.readConfigPath("eUe8vgM3ivJDe"); // Requires Key - eUe8vgM3ivJDe
            /// If Key is > NOT < ^^^ It will return value "Key is wrong.."

	    /// Read Config Values - Will return "Config File Not Found...." if you haven't created config.
	    string username = CF.js("user"); // Will Return Username
	    string password = CF.js("pass"); // Will Return Password

            /// Create Config ["username", "password"] non encrypted
            CF.CreateConfig("mazkdevftest", "passwordi$@£@3£12");
            /// It will create Config file with Eramake ECrypto Encryption
            /// HL7nXvs+j2w4cK0NNs2pUTSDYc4UVJjb628lTn/dvl+tB/QueinrXGdVasfGoPdV1D70dMSOFzbZgAgd6gS10MVksc875+O0vWmA5hFm/6Y=

            Console.ReadLine();
        }
    }
}
```

#### KeyAuth C-Sharp Example - [WORKS ONLY WITH THESE VERSIONS](https://github.com/mazk5145/ConfigSaver/releases)

```cs
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using ConfigSaver;

namespace KeyAuth
{
    class Program
    {
        public static api KeyAuthApp = new api(
            name: "",
            ownerid: "",
            secret: "",
            version: "1.0"
        );

        static string username;
        static string password;
        static string key;

        static void Main(string[] args)
        {

            Console.Title = "Loader";
            Console.WriteLine("\n\n Connecting..");
            KeyAuthApp.init();

            if (!KeyAuthApp.response.success)
            {
                Console.WriteLine("\n Status: " + KeyAuthApp.response.message);
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
            // app data
            Console.WriteLine("\n App data:");
            Console.WriteLine(" Number of users: " + KeyAuthApp.app_data.numUsers);
            Console.WriteLine(" Number of online users: " + KeyAuthApp.app_data.numOnlineUsers);
            Console.WriteLine(" Number of keys: " + KeyAuthApp.app_data.numKeys);
            Console.WriteLine(" Application Version: " + KeyAuthApp.app_data.version);
            Console.WriteLine(" Customer panel link: " + KeyAuthApp.app_data.customerPanelLink);
            KeyAuthApp.check();
            Console.WriteLine($" Current Session Validation Status: {KeyAuthApp.response.message}"); // you can also just check the status but ill just print the message

            if (CF.isExisting())
            {
                KeyAuthApp.login(CF.js("user"), CF.js("pass"));
            } else {
                Console.WriteLine("\n [1] Login\n [2] Register\n [3] Upgrade\n\n Choose option: ");

                int option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        Console.WriteLine("\n\n Enter username: ");
                        username = Console.ReadLine();
                        Console.WriteLine("\n\n Enter password: ");
                        password = Console.ReadLine();
                        KeyAuthApp.login(username, password);
                        break;
                    case 2:
                        Console.WriteLine("\n\n Enter username: ");
                        username = Console.ReadLine();
                        Console.WriteLine("\n\n Enter password: ");
                        password = Console.ReadLine();
                        Console.WriteLine("\n\n Enter license: ");
                        key = Console.ReadLine();
                        KeyAuthApp.register(username, password, key);
                        break;
                    case 3:
                        Console.WriteLine("\n\n Enter username: ");
                        username = Console.ReadLine();
                        Console.WriteLine("\n\n Enter license: ");
                        key = Console.ReadLine();
                        KeyAuthApp.upgrade(username, key);
                        break;
                    default:
                        Console.WriteLine("\n\n Invalid Selection");
                        Thread.Sleep(1500);
                        Environment.Exit(0);
                        break; // no point in this other than to not get error from IDE
                }
            }

            if (!KeyAuthApp.response.success)
            {
                if (CF.isExisting()) { CF.DelConfig(); }

                Console.WriteLine("\n Status: " + KeyAuthApp.response.message);
                Thread.Sleep(1500);
                Environment.Exit(0);
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            { } else {
                CF.CreateConfig(username, password);
            }

            Console.WriteLine("\n Logged In!"); // at this point, the client has been authenticated. Put the code you want to run after here

            // user data
            Console.WriteLine("\n User data:");
            Console.WriteLine(" Username: " + KeyAuthApp.user_data.username);
            Console.WriteLine(" IP address: " + KeyAuthApp.user_data.ip);
            Console.WriteLine(" Hardware-Id: " + KeyAuthApp.user_data.hwid);
            Console.WriteLine(" Created at: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.createdate)));
            if (!String.IsNullOrEmpty(KeyAuthApp.user_data.lastlogin)) // don't show last login on register since there is no last login at that point
                Console.WriteLine(" Last login at: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.lastlogin)));
            Console.WriteLine(" Your subscription(s):");
            for (var i = 0; i < KeyAuthApp.user_data.subscriptions.Count; i++)
            {
                Console.WriteLine(" Subscription name: " + KeyAuthApp.user_data.subscriptions[i].subscription + " - Expires at: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.subscriptions[i].expiry)) + " - Time left in seconds: " + KeyAuthApp.user_data.subscriptions[i].timeleft);
            }

            KeyAuthApp.check();
            Console.WriteLine($" Current Session Validation Status: {KeyAuthApp.response.message}"); // you can also just check the status but ill just print the message
            Console.WriteLine("\n Closing in ten seconds...");
            Thread.Sleep(10000);
            Environment.Exit(0);
        }

        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
            return dtDateTime;
        }
    }
}
```

##### KeyAuth C-Sharp Forms Example - DLL_Version : [1.15 - Release](https://github.com/mazk5145/ConfigSaver/releases/tag/Release_)

```cs
using System.Windows.Forms;
using ConfigSaver; 


namespace KeyAuth
{
    static class Program
    {

        /// <summary>
        /// IF you are using KeyAuth Example non modifed Login form name is Login.cs 
        ///
        /// Login. = Form Name where is KeyAuth Details
        /// Main = Form After Login
        /// </summary>
        static void Main()
        {
            Login.KeyAuthApp.init();
            if (CF.isExisting())
            {
                Login.KeyAuthApp.login(CF.js("user"), CF.js("pass"));
                if (Login.KeyAuthApp.response.success)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Main());
                }
                else
                {
                    CF.DelConfig();
                    api.error($"Status: {Login.KeyAuthApp.response.message}");
                }
            }
            else { }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
```

-----------------------------

#### Created for KeyAuth C-Sharp Loaders

### KeyAuth - [Website](https://keyauth.win) - [Discord](https://keyauth.win/discord)
