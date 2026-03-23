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
                                                Database=Delegacion; xxx
                                                SslMode=None"
         */
#if DEBUG
        public static string CadenaConexion = @"Server=CHAMITO\SQLEXPRESS;
                                        Database=PrestaITESHU;
                                        Trusted_Connection=True;
                                        TrustServerCertificate=True;"; 
#else
        public static string CadenaConexion = @"Server=db16351.databaseasp.net;
                                              Database=db16351;
                                              User Id = db16351; 
                                              Password=pQ!2@5Yw6q_A;Encrypt=False; 
                                              MultipleActiveResultSets=True;";
     
#endif


        public static TipoBD TipoBD = TipoBD.SQLServer;

        public static FabricRepository FabricaRepository = new FabricRepository(CadenaConexion, TipoBD);
    }
}
