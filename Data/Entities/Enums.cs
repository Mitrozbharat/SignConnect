namespace SmartVideoCallApp.Data.Entities
{
    public enum MeetingVisibility
    {
        Public = 1,
        Organization = 2,
        Private = 3
    }

    public enum RoomStatus
    {
        Active = 1,
        Ended = 2
    }

    public enum ActivityType
    {
        RoomCreated = 1,
        JoinRequested = 2,
        JoinApproved = 3,
        JoinRejected = 4,
        UserJoined = 5,
        UserLeft = 6,
        MessageSent = 7,
        SignCoordinatesCaptured = 8
    }
}
