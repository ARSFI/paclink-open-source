namespace Paclink
{
    public struct AccountRecord
    {
        public string Name;               // The account name as set up on the users POP3 Client setup
        public string Password;           // The user Password as set up on the users POP3 Client setup
        public string MimePathIn;         // The directory of the MIME files for messages from this user
        public string MimePathOut;        // The directory of the MIME files for messages to this user
        public string AlternateAddress;   // an alternate (non winlink address for forwarding)
        public string Prefix;             // Optional callsign prefix for ID
        public string Suffix;             // Optional callsign Suffix for ID 
    } // AccountRecord
}
