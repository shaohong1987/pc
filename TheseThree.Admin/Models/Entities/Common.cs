using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheseThree.Admin.Models.Entities
{
    public enum MessageType
    {
        Error=-1,
        Fail=0,
        Success=1
    }

    public enum OrganizationType
    {
        Ward=0,
        Team=1
    }
}