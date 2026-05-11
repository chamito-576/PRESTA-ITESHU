using DAL;

namespace WebApi
{
    public static class Parametros
    {
        //escribir aqui la cadena de conexion 
        /*
         * @"Server=200.79.178.212; 
                                                Port=3036; 
                                                User=root;
                                                Password=2d0Serv3r2025; 
                                                Database=Delegacion; 
                                                SslMode=None"
         */
#if DEBUG
        public static string CadenaConexion = @"SERVER=AISLINN\MSSQLSERVER01; 
                                                DATABASE=PrestaITESHU; 
                                                Integrated Security=True;
                                                TrustServerCertificate=True";
#else
        public static string CadenaConexion = @"Server=db45166.databaseasp.net; 
                                              Database=db45166; 
                                              User Id=db45166; 
                                              Password=2k?GJ8p_9+Am; 
                                              Encrypt=False; 
                                              MultipleActiveResultSets=True;";
     
#endif


        public static TipoBD TipoBD = TipoBD.SQLServer;

        public static FabricRepository FabricaRepository = new FabricRepository(CadenaConexion, TipoBD);
    }
}
