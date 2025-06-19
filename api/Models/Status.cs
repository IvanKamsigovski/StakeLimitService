  public enum Status
    {
        // Ticket can be accepted, since no limit has been applied 
        OK,
        //Ticket can be accepted, but status serves as
        //a warning that device is close to being blocked 
        HOT,
        //Signals that device is blocked from accepting tickets
        BLOCKED,

    }