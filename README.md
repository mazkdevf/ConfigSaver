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

-----------------------------

#### Created for KeyAuth C-Sharp Loaders

### KeyAuth - [Website](https://keyauth.win) - [Discord](https://keyauth.win/discord)
