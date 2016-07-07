using System;

namespace MyWorld.Client.Core
{
	public static class AppSettings
	{
        //Local Service Fabric cluster
        public static string ServerlUrl = "http://192.168.88.214:8740/";

        public static string DefaultTenantId = "MASTER";

        //Remote Service Fabric cluster in Azure's cloud
        //public static string ServerlUrl = "http://TBD/";
    }

}

    