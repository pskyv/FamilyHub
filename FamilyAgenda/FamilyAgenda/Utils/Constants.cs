using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyAgenda.Utils
{
    public static class Constants
    {
        #region firebase
        public const string FirebaseDataBaseUrl = "https://familyagenda-9dcc8.firebaseio.com/";
        public const string FirebaseFCMServerKey = "AAAAFTpZ0PI:APA91bFV6dM_NIQlNWSTIFyXHPrs-8k4AKC7gvIHaBrfliWBqQvbV63gSQb6Wu76zXgEEm61XXq1bWNIzylbfF99srCNyC5ZD22rsD0GNSropgTI-xEE5f6yltZ7RwfBAsLSOdrth40m";
        public const string FirebaseBasePostUrl = "https://fcm.googleapis.com/fcm/";
        public const string FirebaseApiKey = "AIzaSyCZZO4bm-qKbfwpZ2vcviNI2HAAL4jKxl0";

        #endregion

        //syncfusion
        public const string Old_SyncfusionLicenseKey = "MjI0MzBAMzEzNjJlMzIyZTMwTzdkSW5TOXhCeVR6WUVqOStUZ1c2c3FrK2RzZDJ4V01ENTN4dGpkK0dWdz0=;" +
                                                       "MjI0MzFAMzEzNjJlMzIyZTMwQzE4bGx6Wmk0ejdzYlRZUEN5bUljRVltS2lLRU40OHhSMnZIRWFZLzR4bz0=;" +
                                                       "MjI0MzJAMzEzNjJlMzIyZTMwZFMzdWF2ZVpHd3Bhak1JdWtweGJxSHBlZmJwSi8zQjhudkRxcjZVcEFBMD0=;" +
                                                       "MjI0MzNAMzEzNjJlMzIyZTMwbktkMVlPaWVDTGtkREFuVWxjT01lMGJaZ1JjRHp1S0gyS2ZVWCtjTnBZTT0=;" +
                                                       "MjI0MzRAMzEzNjJlMzIyZTMwS1Qrc2ZLWlhlM2prTmdTZmcrVXBFOFJzSDRqWE9vNCs3ZVRnSll4UGFpMD0=;" +
                                                       "MjI0MzVAMzEzNjJlMzIyZTMwQUxkbDZIM2szR1hnY01kQ1JKWGNHRGx0RW9VNjY2dVNHM0h5ZUNHRVQ1ST0=;" +
                                                       "MjI0MzZAMzEzNjJlMzIyZTMwRzh0YUNvZXJuRERaeEhNbHpFQm1Bcy94Y0NXOE83T0lyZ1dlL211Ny80ND0=;" +
                                                       "MjI0MzdAMzEzNjJlMzIyZTMwa29SckJqdm1Ybll2ZWNVN0lkLytJcEl4SGpDbnJpYU9ycTlpME5NY0VzND0=;" +
                                                       "MjI0MzhAMzEzNjJlMzIyZTMwZDJQODFSYzdnTEZQRU42ZGhNMk1LZFY0amVrR0ttZnJKOGlQMU51ckhEND0=";

        public const string SyncfusionLicenseKey = "MjE2ODE5QDMxMzcyZTM0MmUzMGtmZUF5WWZsa2FzYXlac0JTNUp0NzlvenpYSTAvU0lmSU1LNHNPYzJxaGc9";

        //toast messages
        public const string ConnectivityLostMsg = "No internet connection";
        public const string NoConnectionMsg = "Couldn't refresh items, check your connection";
        public const string AuthConnectivityMsg = "Couldn't authenticate, check your connection";
        public const string CouldNotPerformActionMsg = "Couldn't perform action, check your connection";
        public const string AuthenticationFailedMsg = "Authentication failed: ";
    }
}
