using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinlinkServiceClasses
{
    public class WinlinkServiceClasses
    {
    }

    public static class Constants
    {
        public const int MaxAttachmentSize = 120000;
    }

    /*-----------------------------------------------------------------
     * Base request class.
     */
    public abstract class WebServiceRequest
    {
        /// <summary>
        /// Web service access key -- must be provided with every request
        /// </summary>
        public string Key { get; set; }
    }

    public class WebServiceRequestSecure
    {
        public string WebServiceAccessCode { get; set; }
        public string Key { get; set; }
    }

    /*------------------------------------------------------------------
     * base response classes.
     */
    public class ResponseStatus
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public ResponseStatus()
        {
            ErrorCode = "";
            Message = "";
        }
    }

    public class WebServiceResponse
    {
        public ResponseStatus ResponseStatus { get; set; }
        public bool HasError { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public WebServiceResponse()
        {
            HasError = false;
            ResponseStatus = new ResponseStatus();
        }
    }

    public class PingRequest : WebServiceRequest
    {
    }

    public class PingResponse : WebServiceResponse
    {
    }


    public class MessagePickupStationAdd : WebServiceRequest//, IReturn<MessagePickupStationAddResponse>
    {
        public string Requester { get; set; }
        public string Callsign { get; set; }
        public string Password { get; set; }
        public string MpsCallsign { get; set; }
    }

    public class MessagePickupStationAddResponse : WebServiceResponse
    {
    }

    public class MessagePickupStationGet : WebServiceRequest
    {
        public string Requester { get; set; }
        public string Callsign { get; set; }
    }
    public class MessagePickupStationRecord
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string MpsCallsign { get; set; }
    }

    public class MessagePickupStationGetResponse : WebServiceResponse
    {
        public List<MessagePickupStationRecord> MpsList { get; set; }

        public MessagePickupStationGetResponse()
        {
            MpsList = new List<MessagePickupStationRecord>();
        }
    }

    public class MessagePickupStationList : WebServiceRequest
    {
        public string Requester { get; set; }
    }

    public class MessagePickupStationListResponse : WebServiceResponse
    {
        public List<MessagePickupStationRecord> MpsList { get; set; }

        public MessagePickupStationListResponse()
        {
            MpsList = new List<MessagePickupStationRecord>();
        }
    }

    public class MessagePickupStationDelete : WebServiceRequest
    {
        public string Requester { get; set; }
        public string Callsign { get; set; }
        public string Password { get; set; }
    }

    public class MessagePickupStationDeleteResponse : WebServiceResponse
    {
    }

    public class ProgramVersionsLatestResponse : WebServiceResponse
    {
        public List<LatestProgramVersionRecord> ProgramVersionList { get; set; }

        public ProgramVersionsLatestResponse()
        {
            ProgramVersionList = new List<LatestProgramVersionRecord>();
        }
    }
    public enum UserType
    {
        AnyAll,
        Client,
        Sysop
    }

    /*-----------------------
     * System logging error levels.
     */
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    public class LatestProgramVersionRecord
    {
        public string Program { get; set; }
        public string Version { get; set; }
        public UserType UserType { get; set; }

        public LatestProgramVersionRecord(string program, string version, UserType userType)
        {
            Program = program;
            Version = version;
            UserType = userType;
        }
    }
    public class RadioNetworkParamsAddResponse : WebServiceResponse
    {
    }
    public class RadioNetworkParamsGetResponse : WebServiceResponse
    {
        public List<ParamRecord> ParamList { get; set; }

        public RadioNetworkParamsGetResponse()
        {
            ParamList = new List<ParamRecord>();
        }
    }
    public class RadioNetworkParamsDeleteResponse : WebServiceResponse
    {
    }
    public class RadioNetworkParamsListResponse : WebServiceResponse
    {
        public List<ParamRecord> ParamList { get; set; }

        public RadioNetworkParamsListResponse()
        {
            ParamList = new List<ParamRecord>();
        }
    }

    public class ParamRecord
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string Param { get; set; }
        public string Value { get; set; }
    }
    public class HybridStationListResponse : WebServiceResponse
    {
        public List<HybirdRecord> HybirdList { get; set; }

        public HybridStationListResponse()
        {
            HybirdList = new List<HybirdRecord>();
        }
    }

    public class HybirdRecord
    {
        public string Callsign { get; set; }
        public bool AutomaticForwarding { get; set; }
        public bool ManualForwarding { get; set; }
    }

    public class ActivityRecord
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string Source { get; set; }
        public string Channel { get; set; }
        public int ClientType { get; set; }
        public int LinkType { get; set; }
        public int Protocol { get; set; }
        public int MessagesInbound { get; set; }
        public int MessagesOutbound { get; set; }
        public int BytesInbound { get; set; }
        public int BytesOutbound { get; set; }
        public int ConnectTime { get; set; }
    }

    public class DonationRecord
    {
        public string TransactionId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public double Amount { get; set; }
        public string TransactionType { get; set; }
        public string RegistrationKey { get; set; }
        public bool Void { get; set; }
        public string Notes { get; set; }
    }

    public class EmailAliasRecord
    {
        public string Callsign { get; set; }
        public string Alias { get; set; }
        public string Address { get; set; }
    }

    public class IPAddressRecord
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string IPAddress { get; set; }
    }

    public class PositionReportRecord
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string ReportedBy { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Heading { get; set; }
        public string Speed { get; set; }
        public string Comment { get; set; }
        public bool Marine { get; set; }
        public bool Yotreps { get; set; }
        public string LatitudeNMEA { get; set; }
        public string LongitudeNMEA { get; set; }

        public PositionReportRecord()
        {
            Timestamp = DateTime.UtcNow;
            ReportedBy = string.Empty;
            Heading = string.Empty;
            Speed = string.Empty;
            Comment = string.Empty;
            LatitudeNMEA = string.Empty;
            LongitudeNMEA = string.Empty;
        }
    }

    public class SessionRecord
    {
        public DateTime Timestamp { get; set; }
        public string Application { get; set; }
        public string Version { get; set; }
        public string Cms { get; set; }
        public string Server { get; set; }
        public string Client { get; set; }
        public string Sid { get; set; }
        public string Mode { get; set; }
        public int Frequency { get; set; }
        public int Kilometers { get; set; }
        public int Degrees { get; set; }
        public string LastCommand { get; set; }
        public int MessagesSent { get; set; }
        public int MessagesReceived { get; set; }
        public int BytesSent { get; set; }
        public int BytesReceived { get; set; }
        public int HoldingSeconds { get; set; }
        public string IdTag { get; set; }
    }

    public class TrafficRecord
    {
        public DateTime Timestamp { get; set; }
        public string Site { get; set; }
        public string Event { get; set; }
        public string MessageId { get; set; }
        public int ClientType { get; set; }
        public string Callsign { get; set; }
        public string Gateway { get; set; }
        public string Source { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public int Size { get; set; }
        public int Attachments { get; set; }

        public TrafficRecord()
        {
            Timestamp = DateTime.UtcNow;
        }
    }


    public class WhitelistGetResponse : WebServiceResponse
    {
        public List<WhitelistRecord> AccessList { get; set; }

        public WhitelistGetResponse()
        {
            AccessList = new List<WhitelistRecord>();
        }
    }
    public class WhitelistAddResponse : WebServiceResponse
    {
    }
    public class WhitelistDeleteResponse : WebServiceResponse
    {
    }
    public class WhitelistRecord
    {
        public string Address { get; set; }
        public bool Allow { get; set; }
    }
    public class GroupAddressGroupExistsResponse : WebServiceResponse
    {
        public bool GroupExists { get; set; }
    }
    public class GroupAddressAddResponse : WebServiceResponse
    {
    }
    public class GroupAddressDeleteResponse : WebServiceResponse
    {
    }
    public class GroupAddressDeleteGroupResponse : WebServiceResponse
    {
    }
    public class GroupAddressListResponse : WebServiceResponse
    {
        public List<GroupAddressRecord> AddressList { get; set; }

        public GroupAddressListResponse()
        {
            AddressList = new List<GroupAddressRecord>();
        }
    }
    public class GroupAddressGroupCallsignListResponse : WebServiceResponse
    {
        public List<string> GroupList { get; set; }

        public GroupAddressGroupCallsignListResponse()
        {
            GroupList = new List<string>();
        }
    }
    public class GroupAddressRecord
    {
        public string GroupCallsign { get; set; }
        public string Address { get; set; }
        public string SubjectContains { get; set; }
        public string EnteredBy { get; set; }
    }
    public class GatewayChannelCountResponse : WebServiceResponse
    {
        public int GatewayCount { get; set; }
    }
    public class GatewayStatusResponse : WebServiceResponse
    {
        public List<GatewayStatusRecord> Gateways { get; set; }

        public GatewayStatusResponse()
        {
            Gateways = new List<GatewayStatusRecord>();
        }
    }

    public class GatewayStatusRecord
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string BaseCallsign { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int HoursSinceStatus { get; set; }
        public string LastStatus { get; set; }
        public string Comments { get; set; }
        public GatewayOperatingMode RequestedMode { get; set; }
        public List<GatewayChannelRecord> GatewayChannels { get; set; }

        public GatewayStatusRecord()
        {
            GatewayChannels = new List<GatewayChannelRecord>();
        }
    }

    public class GatewayChannelReportResponse : WebServiceResponse
    {
        public List<GatewayChannelReportRecord> Channels { get; set; }

        public GatewayChannelReportResponse()
        {
            Channels = new List<GatewayChannelReportRecord>();
        }
    }

    public class GatewayChannelReportRecord
    {
        public string Callsign { get; set; }
        public string Gridsquare { get; set; }
        public int Frequency { get; set; }
        public int Mode { get; set; }
        public string Hours { get; set; }
        public string ServiceCode { get; set; }
     }

    //-------------------------------------------------------------------------------------

    public class GatewayChannelRecord
    {
        public DateTime Timestamp { get; set; }
        public string SupportedModes { get; set; }
        public int Mode { get; set; }
        public string Gridsquare { get; set; }
        public int Frequency { get; set; }
        public string OperatingHours { get; set; }
        public string Baud { get; set; }
        public string RadioRange { get; set; }
        public string Antenna { get; set; }
        public string ServiceCode { get; set; }
    }

    public enum GatewayOperatingMode
    {
        AnyAll,
        Packet,
        Pactor,
        Winmor,
        RobustPacket,
        AllHf
    }
    public class SysopListResponse : WebServiceResponse
    {
        public List<SysopRecord> SysopList { get; set; }

        public SysopListResponse()
        {
            SysopList = new List<SysopRecord>();
        }
    }
    public class SysopGetResponse : WebServiceResponse
    {
        public SysopRecord Sysop { get; set; }

        public SysopGetResponse()
        {
            Sysop = new SysopRecord();
        }
    }
    public class SysopAddResponse : WebServiceResponse
    {
    }
    public class SysopDeleteResponse : WebServiceResponse
    {
    }

    public class SysopRecord
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string GridSquare { get; set; }
        public string SysopName { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Phones { get; set; }
        public string Website { get; set; }
        public string Comments { get; set; }
    }

    public class GatewayNotifyRecord
    {
        public DateTime Timestamp { get; set; }
        public string GatewayCallsign { get; set; }
        public string Callsign { get; set; }
        public int OfflineNotification { get; set; }
        public string NotificationEmail { get; set; }
    }

    public class InquiriesCatalogResponse : WebServiceResponse
    {
        public List<InquiryRecord> Inquiries { get; set; }

        public InquiriesCatalogResponse()
        {
            Inquiries = new List<InquiryRecord>();
        }
    }

    public class InquiryRecord
    {
        public DateTime Timestamp { get; set; }
        public string InquiryId { get; set; }
        public string Category { get; set; }
        public string Subject { get; set; }
        public string Process { get; set; }
        public string Url { get; set; }
        public int Lifetime { get; set; }
        public int SizeEstimate { get; set; }
        public bool Enabled { get; set; }
    }

    public enum ClientType
    {
        Unknown,
        Webmail,
        Simple,
        Airmail,
        Paclink,
        FBB,
        Browser,
        Outpost,
        Relay,
        Express,
        SNOS,
        BPQ,
        JNOS,
        DRATS,
        UnixLINK,
        Pat
    }

    public enum EventType
    {
        Unknown,
        Accepted,
        Forwarded,
        Received,
        Rejected,
        Proposed,
        Processed,
        Posted
 }

    public enum TrafficType
    {
        Unknown,
        P2P,
        RadioOnly
    }

    public class TrafficAdd : WebServiceRequest
    {
        public DateTime TimeStamp { get; set; }
        public string MessageId { get; set; }
        public string Callsign { get; set; }
        public int Frequency { get; set; }
        public ClientType Client { get; set; }
        public EventType Event { get; set; }
        public TrafficType TrafficType { get; set; }
        public string Mime { get; set; }
    }

    public class TrafficAddResponse : WebServiceResponse
    {
    }
    public class VersionAdd : WebServiceRequest
    {
        public string Callsign { get; set; }
        public string Program { get; set; }
        public string Version { get; set; }
        public string Comments { get; set; }

        public VersionAdd()
        {
            Comments = string.Empty;
        }
    }

    public class VersionAddResponse : WebServiceResponse
    {
    }

    public class VersionDelete : WebServiceRequest
    {
        public string Callsign { get; set; }
        public string Program { get; set; }
    }

    public class VersionDeleteResponse : WebServiceResponse
    {
    }

    public class VersionList : WebServiceRequest
    {
        public string Program { get; set; }
        public int HistoryHours { get; set; }
    }

    public class VersionListResponse : WebServiceResponse
    {
        public List<VersionRecord> VersionList { get; set; }

        public VersionListResponse()
        {
            VersionList = new List<VersionRecord>();
        }
    }

    public class VersionRecord
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string Program { get; set; }
        public string Version { get; set; }
        public string Comments { get; set; }
    }

    public class ChannelRecord
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string BaseCallsign { get; set; }
        public string GridSquare { get; set; }
        public int Frequency { get; set; }
        public int Mode { get; set; }
        public int Baud { get; set; }
        public int Power { get; set; }
        public int Height { get; set; }
        public int Gain { get; set; }
        public int Direction { get; set; }
        public string OperatingHours { get; set; }
        public string ServiceCode { get; set; }
    }

    public class WatchRecord
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string Program { get; set; }
        public int AllowedTardyHours { get; set; }
        public string NotificationEmails { get; set; }
    }

    public class ChannelList : WebServiceRequest
    {
        public List<int> Modes { get; set; }
        public int HistoryHours { get; set; }
        public string ServiceCodes { get; set; }
    }

    public class ChannelListResponse : WebServiceResponse
    {
        public List<ChannelRecord> Channels { get; set; }

        public ChannelListResponse()
        {
            Channels = new List<ChannelRecord>();
        }
    }

    public class ChannelGet : WebServiceRequest
    {
        public string Callsign { get; set; }
    }

    public class ChannelGetResponse : WebServiceResponse
    {
        public List<ChannelRecord> Channels { get; set; }

        public ChannelGetResponse()
        {
            Channels = new List<ChannelRecord>();
        }
    }

    public class ChannelAdd : WebServiceRequest
    {
        public string Callsign { get; set; }
        public string BaseCallsign { get; set; }
        public string GridSquare { get; set; }
        public double Frequency { get; set; }
        public int Mode { get; set; }
        public int Baud { get; set; }
        public int Power { get; set; }
        public int Height { get; set; }
        public int Gain { get; set; }
        public int Direction { get; set; }
        public string Hours { get; set; }
        public string ServiceCode { get; set; }

        public ChannelAdd()
        {
            Hours = string.Empty;
            ServiceCode = "PUBLIC";
        }
    }

    public class ChannelAddResponse : WebServiceResponse
    {
    }

    public class ChannelDelete : WebServiceRequest
    {
        public string Callsign { get; set; }
        public double Frequency { get; set; }
        public int Mode { get; set; }
    }

    public class ChannelDeleteResponse : WebServiceResponse
    {
    }

    public class ChannelDeleteAll : WebServiceRequest
    {
        public string Callsign { get; set; }
    }

    public class ChannelDeleteAllResponse : WebServiceResponse
    {
    }

    public class WatchSet : WebServiceRequest
    {
        public string Callsign { get; set; }
        public string Password { get; set; }
        public string Program { get; set; }
        public int AllowedTardyHours { get; set; }
        public string NotificationEmails { get; set; }
    }

    public class WatchSetResponse : WebServiceResponse
    {
    }

    public class WatchDelete : WebServiceRequest
    {
        public string Callsign { get; set; }
        public string Password { get; set; }
        public string Program { get; set; }
    }
    public class WatchDeleteResponse : WebServiceResponse
    {
    }
    
    public class WatchList: WebServiceRequest
    {
        public string Callsign { get; set; }
        public string Password { get; set; }
    }

    public class WatchListResponse : WebServiceResponse
    {
        public List<WatchRecord> List { get; set; }
        public WatchListResponse()
        {
            List = new List<WatchRecord>();
        }
    }

    public class WatchGet : WebServiceRequest
    {
        public string Callsign { get; set; }
        public string Password { get; set; }
        public string Program { get; set; }
    }

    public class WatchGetResponse : WebServiceResponse
    {
        public DateTime Timestamp { get; set; }
        public string Callsign { get; set; }
        public string Program { get; set; }
        public int AllowedTardyHours { get; set; }
        public string NotificationEmails { get; set; }
    }
    public class WatchPing : WebServiceRequest
    {
        public string Callsign { get; set; }
        public string Program { get; set; }
    }
    public class WatchPingResponse : WebServiceResponse
    {
    }

     public class SessionRecordAdd : WebServiceRequest
    {
        public DateTime Timestamp { get; set; }
        public string Application { get; set; }
        public string Version { get; set; }
        public string Cms { get; set; }
        public string Server { get; set; }
        public string Client { get; set; }
        public string Sid { get; set; }
        public string Mode { get; set; }
        public int Frequency { get; set; }
        public int Kilometers { get; set; }
        public int Degrees { get; set; }
        public string LastCommand { get; set; }
        public int MessagesSent { get; set; }
        public int MessagesReceived { get; set; }
        public int BytesSent { get; set; }
        public int BytesReceived { get; set; }
        public int HoldingSeconds { get; set; }
        public string IdTag { get; set; }

        public SessionRecordAdd()
        {
            Timestamp = DateTime.UtcNow;
            Application = string.Empty;
            Version = string.Empty;
            Cms = string.Empty;
            Server = string.Empty;
            Client = string.Empty;
            Sid = string.Empty;
            Mode = string.Empty;
            Frequency = 0;
            Kilometers = -1;
            Degrees = -1;
            LastCommand = string.Empty;
            MessagesSent = 0;
            MessagesReceived = 0;
            BytesSent = 0;
            BytesReceived = 0;
            HoldingSeconds = 0;
            IdTag = string.Empty;
        }
    }

    public class SessionRecordAddResponse : WebServiceResponse
    {
    }

    public class AccountAdd : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public string Password { get; set; }
        public string CallsignPrefix { get; set; }
        public string CallsignSuffix { get; set; }
        public bool IsTactical { get; set; }
        public bool IsGroupAddress { get; set; }
        public string AlternateEmailAddress { get; set; }
        public string PasswordRecoveryAddress { get; set; }
        public bool NoPurge { get; set; }
        public bool GetewayAccess { get; set; }
        public bool LockedOut { get; set; }
        public int MaxMessgeSize { get; set; }
        public int PendingMessages { get; set; }
        public DateTime LastAccess { get; set; }
        public List<ActivityRecord> Activity { get; set; }
        public List<DonationRecord> Donations { get; set; }
        public List<EmailAliasRecord> EmailAliasList { get; set; }
        public List<ChannelRecord> GatewayChannels { get; set; }
        public List<GroupAddressRecord> GroupAddresses { get; set; }
        public List<IPAddressRecord> IPAddresses { get; set; }
        public List<MessagePickupStationRecord> MpsList { get; set; }
        public List<PositionReportRecord> PositionReports { get; set; }
        public List<SessionRecord> SessionRecords { get; set; }
        public SysopRecord SysopData { get; set; }
        public List<TrafficRecord> TrafficLogs { get; set; }
        public List<VersionRecord> VersionList { get; set; }
        public List<WhitelistRecord> WhiteList { get; set; }
    }

    public class AccountAddResponse : WebServiceResponse
    {
    }

    public class AccountPasswordChange : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class AccountPasswordChangeResponse : WebServiceResponse
    {
    }

    public class AccountRemove : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
    }

    public class AccountRemoveResponse : WebServiceResponse
    {
    }

    public class AccountExists : WebServiceRequest
    {
        public string Callsign { get; set; }
    }

    public class AccountExistsResponse : WebServiceResponse
    {
        public bool CallsignExists { get; set; }
    }

    public class AccountIsGroupAddress : WebServiceRequest
    {
        public string Callsign { get; set; }
    }

    public class AccountIsGroupAddressResponse : WebServiceResponse
    {
        public bool IsGroupAddress { get; set; }
    }

    public class AccountPasswordGet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
    }

    public class AccountPasswordGetResponse : WebServiceResponse
    {
        public string Password { get; set; }
    }

    public class AccountPasswordSet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public string Password { get; set; }
    }

    public class AccountPasswordSetResponse : WebServiceResponse
    {
    }

    public class AccountPasswordSend : WebServiceRequest
    {
        public string Callsign { get; set; }
    }

    public class AccountPasswordSendResponse : WebServiceResponse
    {
        public string ResponseMessage { get; set; }
    }

    public class AccountPasswordValidate : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public string Password { get; set; }
    }

    public class AccountPasswordValidateResponse : WebServiceResponse
    {
        public bool IsValid { get; set; }
    }

    public class AccountPasswordListResponse : WebServiceResponse
    {
        public List<PasswordRecord> Passwords { get; set; }

        public AccountPasswordListResponse()
        {
            Passwords = new List<PasswordRecord>();
        }
    }

    public class PasswordRecord
    {
        public string Callsign { get; set; }
        public string Password { get; set; }
    }

    public class AccountNoPurgeGet : WebServiceRequest
    {
        public string Callsign { get; set; }
    }

    public class AccountNoPurgeGetResponse : WebServiceResponse
    {
        public bool NoPurge { get; set; }
    }

    public class AccountNoPurgeSet : WebServiceRequest
    {
        public string Callsign { get; set; }
        public bool NoPurge { get; set; }
    }

    public class AccountNoPurgeSetResponse : WebServiceResponse
    {
    }

    public class AccountGatewayAccessGet : WebServiceRequest
    {
        public string Callsign { get; set; }
    }

    public class AccountGatewayAccessGetResponse : WebServiceResponse
    {
        public bool GatewayAccess { get; set; }
    }

    public class AccountGatewayAccessSet : WebServiceRequest
    {
        public string Callsign { get; set; }
        public bool GatewayAccess { get; set; }
    }

    public class AccountGatewayAccessSetResponse : WebServiceResponse
    {
    }

    public class AccountGatewayAccessList : WebServiceRequest
    {
    }

    public class AccountGatewayAccessListResponse : WebServiceResponse
    {
        public List<string> GatewayAuthorized { get; set; }

        public AccountGatewayAccessListResponse()
        {
            GatewayAuthorized = new List<string>();
        }
    }

    public class AccountPasswordRecoveryEmailGet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
    }

    public class AccountPasswordRecoveryEmailGetResponse : WebServiceResponse
    {
        public string RecoveryEmail { get; set; }
    }

    public class AccountPasswordRecoveryEmailSet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public string RecoveryEmail { get; set; }
    }

    public class AccountPasswordRecoveryEmailSetResponse : WebServiceResponse
    {
    }

    public class AccountLastAccessGet : WebServiceRequest
    {
        public string Callsign { get; set; }
    }

    public class AccountLastAccessGetResponse : WebServiceResponse
    {
        public DateTime LastAccess { get; set; }
    }

    public class AccountLockedOutGet : WebServiceRequest
    {
        public string Callsign { get; set; }
    }

    public class AccountLockedOutGetResponse : WebServiceResponse
    {
        public bool LockedOut { get; set; }
    }

    public class AccountLockedOutSet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public bool LockedOut { get; set; }
    }

    public class AccountLockedOutSetResponse : WebServiceResponse
    {
    }

    public class AccountAlternateEmailGet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
    }

    public class AccountAlternateEmailGetResponse : WebServiceResponse
    {
        public string AlternateEmail { get; set; }
    }

    public class AccountAlternateEmailSet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public string AlternateEmail { get; set; }
    }

    public class AccountAlternateEmailSetResponse : WebServiceResponse
    {
    }

    public class AccountCallsignPrefixGet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
    }

    public class AccountCallsignPrefixGetResponse : WebServiceResponse
    {
        public string CallsignPrefix { get; set; }
    }

    public class AccountCallsignPrefixSet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public string CallsignPrefix { get; set; }
    }

    public class AccountCallsignPrefixSetResponse : WebServiceResponse
    {
    }

    public class AccountCallsignSuffixGet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
    }

    public class AccountCallsignSuffixGetResponse : WebServiceResponse
    {
        public string CallsignSuffix { get; set; }
    }

    public class AccountCallsignSuffixSet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public string CallsignSuffix { get; set; }
    }

    public class AccountCallsignSuffixSetResponse : WebServiceResponse
    {
    }

    public class AccountMaxMessageSizeGet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
    }

    public class AccountMaxMessageSizeGetResponse : WebServiceResponse
    {
        public int MaxMessageSize { get; set; }
    }

    public class AccountMaxMessageSizeSet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public int MaxMessageSize { get; set; }

        public AccountMaxMessageSizeSet()
        {
            MaxMessageSize = Constants.MaxAttachmentSize;
        }
    }

    public class AccountMaxMessageSizeSetResponse : WebServiceResponse
    {
    }

    public class AccountTacticalExists : WebServiceRequestSecure
    {
        public string TacticalAccount { get; set; }
    }

    public class AccountTacticalExistsResponse : WebServiceResponse
    {
        public bool Tactical { get; set; }
    }

    public class AccountTacticalAdd : WebServiceRequestSecure
    {
        public string TacticalAccount { get; set; }
    }

    public class AccountTacticalAddResponse : WebServiceResponse
    {
    }

    public class AccountTacticalRemove : WebServiceRequestSecure
    {
        public string TacticalAccount { get; set; }
    }

    public class AccountTacticalRemoveResponse : WebServiceResponse
    {
    }

#if false
    public class AccountSettingsGet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
    }

    public class AccountSettingsGetResponse : WebServiceResponse
    {
        public string Callsign { get; set; }
        public DateTime LastAccess { get; set; }
        public string Password { get; set; }
        public string RecoveryEmail { get; set; }
        public string CallsignPrefix { get; set; }
        public string CallsignSuffix { get; set; }
        public int MaxMessageSize { get; set; }
        public bool NoPurge { get; set; }
        public bool GatewayAccess { get; set; }
        public bool SecureLogon { get; set; }
        public bool LockedOut { get; set; }
        public string AlternateEmail { get; set; }
        public bool Tactical { get; set; }
        public bool GroupAddress { get; set; }
    }

    public class CallsignPropertiesSet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public string CallsignPrefix { get; set; }
        public string CallsignSuffix { get; set; }
        public int MaxMessageSize { get; set; }
        public bool GatewayAccess { get; set; }
        public bool SecureLogon { get; set; }
        public bool NoPurge { get; set; }

        public CallsignPropertiesSet()
        {
            CallsignPrefix = string.Empty;
            CallsignSuffix = string.Empty;
            MaxMessageSize = Constants.MaxAttachmentSize;
        }
    }

    public class CallsignPropertiesSetResponse : WebServiceResponse
    {
    }

    public class AccountLoginBypassGet : WebServiceRequest
    {
        public string Callsign { get; set; }
    }

    public class AccountLoginBypassGetResponse : WebServiceResponse
    {
        public bool LoginBypass { get; set; }
    }

    public class AccountLoginBypassSet : WebServiceRequestSecure
    {
        public string Callsign { get; set; }
        public bool LoginBypass { get; set; }
    }

    public class AccountLogonBypassSetResponse : WebServiceResponse
    {
    }

#endif
    public class AccountCallsignSearch : WebServiceRequestSecure
    {
        public string SearchText { get; set; }
    }

    public class AccountCallsignSearchResponse : WebServiceResponse
    {
        public List<string> Callsigns { get; set; }

        public AccountCallsignSearchResponse()
        {
            Callsigns = new List<string>();
        }
    }

    public class ArsfiRegistrationValid : WebServiceRequest
    {
        public string Callsign { get; set; }
        public string RegistrationKey { get; set; }
    }

    public class ArsfiRegistrationValidResponse : WebServiceResponse
    {
        public bool Valid { get; set; }
    }

    public class AvailabilityTest : WebServiceRequest
    {
        public string AdditionalData { get; set; }
    }

    public class AvailabilityTestResponse : WebServiceResponse
    {
        public string SiteName { get; set; }
        public bool ServerOnline { get; set; }
        public bool DatabaseOnline { get; set; }
        public bool TelentServerOnline { get; set; }
        public string AdditionalData { get; set; }
        public DateTime Timestamp { get; set; }

        public AvailabilityTestResponse()
        {
            Timestamp = DateTime.UtcNow;
        }
    }

    public class AvailabilityRequestRate : WebServiceRequest
    {
    }

    public class AvailabilityRequestRateResponse : WebServiceResponse
    {
        public long RequestsPerMinute { get; set; }
        public DateTime Timestamp { get; set; }

        public AvailabilityRequestRateResponse()
        {
            Timestamp = DateTime.UtcNow;
        }
    }

    public class AvailabilityRequestCount : WebServiceRequest
    {
    }

    public class AvailabilityRequestCountResponse : WebServiceResponse
    {
        public List<AvailabilityCountRecord> RequestCounts { get; set; }
        public DateTime Timestamp { get; set; }

        public AvailabilityRequestCountResponse()
        {
            RequestCounts = new List<AvailabilityCountRecord>();
            Timestamp = DateTime.UtcNow;
        }
    }

    public class AvailabilityCountRecord
    {
        public string Ip { get; set; }
        public int Count { get; set; }
    }

    public class PasswordsList : WebServiceRequestSecure
    {
        public string Challenge { get; set; }
    }

    public class PasswordsListResponse : WebServiceResponse
    {
        public List<PasswordHashRecord> PasswordHash { get; set; }

        public PasswordsListResponse()
        {
            PasswordHash = new List<PasswordHashRecord>();
        }
    }

    public class PasswordHashRecord
    {
        public string Callsign { get; set; }
        public string PasswordHash { get; set; }
    }

    public class MessageSend : WebServiceRequestSecure
    {
        public string MessageId { get; set; }
        public string Mime { get; set; }
    }

    public class MessageSendResponse : WebServiceResponse
    {
        public string MessageId { get; set; }
    }

    public class MessageSendText : WebServiceRequestSecure
    {
        public string MessageId { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public List<String> ToAddresses { get; set; }
        public List<String> CcAddresses { get; set; }
        public string Body { get; set; }
        public List<Attachment> Attachments { get; set; }

        public MessageSendText()
        {
            Attachments = new List<Attachment>();
            ToAddresses = new List<string>();
            CcAddresses = new List<string>();
            Body = string.Empty;
        }
    }

    public class Attachment
    {
        public string FileName { get; set; }
        public Byte[] Image { get; set; }
    }

    public class MessageSendTextResponse : WebServiceResponse
    {
        public string MessageId { get; set; }
    }

    public class MessageSendSimple : WebServiceRequestSecure
    {
        public string MessageId { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
    }

    public class MessageSendSimpleResponse : WebServiceResponse
    {
        public string MessageId { get; set; }
    }

    public class MessageGet : WebServiceRequestSecure
    {
        public string AccountName { get; set; }
        public string Password { get; set; }
        public string MessageId { get; set; }
        public bool MarkAsForwarded { get; set; }
    }

    public class MessageGetResponse : WebServiceResponse
    {
        public string MessageId { get; set; }
        public string Mime { get; set; }
    }

    public class MessageGetDecoded : WebServiceRequestSecure
    {
        public string AccountName { get; set; }
        public string Password { get; set; }
        public string MessageId { get; set; }
        public bool MarkAsForwarded { get; set; }
    }

    public class MessageGetDecodedResponse : WebServiceResponse
    {
        public string MessageId { get; set; }
        public string Header { get; set; }
        public string Sender { get; set; }
        public string Source { get; set; }
        public string Subject { get; set; }
        public List<String> ToAddresses { get; set; }
        public List<String> CcAddresses { get; set; }
        public string Body { get; set; }
        public List<Attachment> Attachments { get; set; }
        public bool Forwarded { get; set; }
        public string Mime { get; set; }

        public MessageGetDecodedResponse()
        {
            Attachments = new List<Attachment>();
            ToAddresses = new List<string>();
            CcAddresses = new List<string>();
            Body = string.Empty;
        }
    }

    public class MessageDelete : WebServiceRequestSecure
    {
        public string MessageId { get; set; }
        public bool CompleteDeletion { get; set; }
        public MessageDelete()
        {
            CompleteDeletion = false;
        }
    }

    public class MessageDeleteResponse : WebServiceResponse
    {
    }

    public class MessageList : WebServiceRequestSecure
    {
        public string AccountName { get; set; }
        public string Password { get; set; }
    }

    public class MessageListResponse : WebServiceResponse
    {
        public List<MessageListRecord> Messages { get; set; }

        public MessageListResponse()
        {
            Messages = new List<MessageListRecord>();
        }
    }

    public class MessageListRecord
    {
        public string MessageId { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public int Size { get; set; }
        public int Attachments { get; set; }
    }

    public class MessageExists : WebServiceRequestSecure
    {
        public string MessageId { get; set; }
    }

    public class MessageExistsResponse : WebServiceResponse
    {
        public bool Exists { get; set; }
    }

    public enum MessageQueryType
    {
        None,
        Action,
        Addressee,
        Cms,
        Gateway,
        MessageId,
        Sender,
        Source,
        Subject,
        UserNotices
    }

    public class MessageListQueryRecord
    {
        public DateTime Timestamp { get; set; }
        public string MessageId { get; set; }
        public int Size { get; set; }
        public int Attachments { get; set; }
        public string Action { get; set; }
        public string Source { get; set; }
        public string Cms { get; set; }
        public string Gateway { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
    }

    public class DeliveryListRecord
    {
        public string Address { get; set; }
        public bool Forwarded { get; set; }
    }

    public class MessageListQuery : WebServiceRequest
    {
        public MessageQueryType QueryType { get; set; }
        public string QueryArgument { get; set; }
        public int HistoryHours { get; set; }
        public int RecordLimit { get; set; }

        public MessageListQuery()
        {
            QueryType = MessageQueryType.None;
            HistoryHours = 24;
            RecordLimit = 1000;
        }
    }

    public enum UserNoticeGroup
    {
        None,
        All,
        ArmyAirForce,
        CadetForces,
        GatewayAuthorized,
        NavyMarine,
        NonGatewayAuthorized,
        NonUSAmateur,
        USAmateur
    }


    public class MessageListQueryResponse : WebServiceResponse
    {
        public List<MessageListQueryRecord> MessageList { get; set; }

        public MessageListQueryResponse()
        {
            MessageList = new List<MessageListQueryRecord>();
        }
    }

    public class MessageDeliveryList : WebServiceRequest
    {
        public string MessageId { get; set; }
    }

    public class MessageDeliveryListResponse : WebServiceResponse
    {
        public List<DeliveryListRecord> DeliveryList { get; set; }

        public MessageDeliveryListResponse()
        {
            DeliveryList = new List<DeliveryListRecord>();
        }
    }

    public class MessageReprocess : WebServiceRequest
    {
        public string MessageId { get; set; }
    }

    public class MessageReprocessResponse : WebServiceResponse
    {
    }

    public class MessageUserNotice : WebServiceRequest
    {
        public static string UserNoticeSubject = "//WL2K User Notice";

        public UserNoticeGroup Group { get; set; }
        public string UserNotice { get; set; }
        public int ActiveDays { get; set; }
        public string TestCallsign { get; set; }

        public MessageUserNotice()
        {
            Group = UserNoticeGroup.None;
            ActiveDays = 21;
            UserNotice = string.Empty;
            TestCallsign = string.Empty;
        }
    }

    public class MessageUserNoticeResponse : WebServiceResponse
    {
    }

    public class MeshNodeMasterRecord : WebServiceResponse
    {
        public string node { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string api_version { get; set; }
        public int chanbw { get; set; }
        public string grid_square { get; set; }
        public int active_tunnel_count { get; set; }
        public int channel { get; set; }
        public string model { get; set; }
        public string board_id { get; set; }
        public string firmware_mfg { get; set; }
        public string firmware_version { get; set; }
        public List<MeshNodeRecord> hosts { get; set; }

        public MeshNodeMasterRecord()
        {
            hosts = new List<MeshNodeRecord>();
        }
    }

    public class MeshNodeRecord
    {
        public string name { get; set; }
        public string ip { get; set; }
    }

    public class MeshNodeServicesRecord : WebServiceResponse
    {
        public string node { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string api_version { get; set; }
        public int chanbw { get; set; }
        public string grid_square { get; set; }
        public int active_tunnel_count { get; set; }
        public int channel { get; set; }
        public string model { get; set; }
        public string board_id { get; set; }
        public string firmware_mfg { get; set; }
        public string firmware_version { get; set; }
        public List<MeshNodeService> services { get; set; }

        public MeshNodeServicesRecord()
        {
            services = new List<MeshNodeService>();
        }
   }

    public class MeshNodeService
    {
        public string name { get; set; }
        public string protocol { get; set; }
        public string link { get; set; }
    }

    public class MeshNodeEntry
    {
        public string DisplayName { get; set; }
        public string ServiceName { get; set; }
        public string ip { get; set; }
        public string ServiceLink { get; set; }
        public string port { get; set; }

        public MeshNodeEntry()
        {
            DisplayName = "";
            ServiceName = "";
            ip = "";
            ServiceLink = "";
        }
            
    }

    public class ProgramVersionsRequest : WebServiceRequest
    {
        public UserType UserType { get; set; }
        public ProgramVersionsRequest()
        {
            UserType = UserType.AnyAll;
        }
    }

    public class ProgramVersionsResponse : WebServiceResponse
    {
        public List<ProgramVersionsRecord> ProgramVersionList { get; set; }

        public ProgramVersionsResponse()
        {
            ProgramVersionList = new List<ProgramVersionsRecord>();
        }
    }

    public class ProgramVersionsRecord
    {
        public string Callsign { get; set; }
        public string Program { get; set; }
        public string Version { get; set; }
        public string CurrentVersion { get; set; }
        public UserType UserType { get; set; }
        public bool UpdateNeeded { get; set; }
    }


}
