using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace eDnevnikDLL
{
    public static class VezaSaBazom
    {
        //static SqlConnection Cn = new SqlConnection("server=.;integrated security=false;password=Lozinka.1; database=eDnevnik");
        static SqlConnection Cn = new SqlConnection("Server=.;Database=eDnevnikRG;User Id=sa;Password=Lozinka.1");
        

        #region ProfesorCRUD
        public static int DodavanjeProfesora(Profesor dodati) //Da li ce mo da vracamo i poruku ili ne???
        {
            //Validate first
            var responseFromValidator = Check.DataAnnotation.ValidateEntity<Profesor>(dodati);
            if (responseFromValidator.HasError)
            {
                /*foreach (var error in responseFromValidator.ValidationErrors)
                {
                    whatError = whatError + error + System.Environment.NewLine;           <<< ???
                }*/
                return 99;
            }


            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "skola.ProfesoriUpisNovih";

                int Ret = 99;
              
                Cm.Parameters.Add(new SqlParameter("@ImeProfesora", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.ImeProfesora));
                Cm.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.Email));
                Cm.Parameters.Add(new SqlParameter("@KontaktTelefon", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.KontaktTelefon));
                Cm.Parameters.Add(new SqlParameter("@LoginSifra", SqlDbType.NVarChar, 4000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.LoginSifra));
                Cm.Parameters.Add(new SqlParameter("@Admin", SqlDbType.Bit, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.Admin));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                Cm.ExecuteNonQuery();
                Cn.Close();

                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                return Ret;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 99;
            }
        }
        
        public static int IzmenaProfesora(Profesor izmeniti)
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "dbo.profesoriUPDATE";

                int Ret = 99;

                Cm.Parameters.Add(new SqlParameter("@ImeProfesora", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.ImeProfesora));
                Cm.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.Email));
                Cm.Parameters.Add(new SqlParameter("@KontaktTelefon", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.KontaktTelefon));
                Cm.Parameters.Add(new SqlParameter("@LoginSifra", SqlDbType.NVarChar, 4000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.LoginSifra));
                Cm.Parameters.Add(new SqlParameter("@Admin", SqlDbType.Bit, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.Admin));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                Cm.ExecuteNonQuery();
                Cn.Close();


                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                return Ret;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 99;
            }
        }
        public static int BrisanjeProfesora(Profesor izbrisati)
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "dbo.profesoriDELETE";

                int Ret = 99;

                Cm.Parameters.Add(new SqlParameter("@ProfesorID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izbrisati.ProfesorID));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                Cm.ExecuteNonQuery();
                Cn.Close();


                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                return Ret;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 99;
            }
        }

        public static List<Profesor> IzlistavanjeProfesora()
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "skola.ProfesoriIzlistavanje";

                //int Ret = 99;

               // Cm.Parameters.Add(new SqlParameter("@ProfesorID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izbrisati.ProfesorID));
                //Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));


                Cn.Open();
                List<Profesor> profesori = new List<Profesor>();
                
                SqlDataReader dR = Cm.ExecuteReader();

                while(dR.Read())
                {
                    Profesor p = new Profesor(dR.GetInt32(0),dR.GetString(1),dR.GetString(2),dR.GetString(3),dR.GetString(4),dR.GetBoolean(5));
                    profesori.Add(p);
                }

                Cn.Close();

                Console.WriteLine(profesori.Count);
                //Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                //return Ret;
                return profesori;

            }
            catch (Exception ex)
            {
                Cn.Close();
                Console.WriteLine(ex);
                return null;
            }
        }



        #endregion

        #region OdeljenjeCRUD
        public static int DodavanjeOdeljenja(Odeljenja dodati) //VEROVATNO NE TREBA
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "dbo.odeljenjaUPDATE";

                int Ret = 99;

                Cm.Parameters.Add(new SqlParameter("@OdeljenjeID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.OdeljenjeID));
                Cm.Parameters.Add(new SqlParameter("@RazredniID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.RazredniID));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                Cm.ExecuteNonQuery();
                Cn.Close();

                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                return Ret;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 99;
            }
        }
        public static int IzlistavanjeOdeljenja(Odeljenja select, int BrojPoStrani, int TrenutnaStrana)
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "dbo.odeljenjaSELECT";

                int Ret = 99;
                //@BrojPoStrani int = 20, @TrenutnaStrana int, @BrojOdeljenja int, @GodinaSkolovanja int, @SkolskaGodina int = year
                Cm.Parameters.Add(new SqlParameter("@BrojPoStrani", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, BrojPoStrani));
                Cm.Parameters.Add(new SqlParameter("@TrenutnaStrana", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, TrenutnaStrana));
                Cm.Parameters.Add(new SqlParameter("@BrojOdeljenja", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, select.RedniBroj));
                Cm.Parameters.Add(new SqlParameter("@GodinaSkolovanja", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, select.RazredniID));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                Cm.ExecuteNonQuery();
                Cn.Close();

                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                return Ret;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 99;
            }
        }
        
        //FALI PRIKAZ ODELJENJA
        #endregion

        #region OceneCRUD
        public static int DodavanjeOcena(Ocena dodati)
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "dbo.OceneINSERT";

                int Ret = 99;
       
                Cm.Parameters.Add(new SqlParameter("@TipOcene", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.TipOcene));
                Cm.Parameters.Add(new SqlParameter("@Ocena", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.OcenaVrednost));
                Cm.Parameters.Add(new SqlParameter("@OpisOcene", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.OpisOcene));
                Cm.Parameters.Add(new SqlParameter("@MaticniBroj", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.MaticniBroj));
                Cm.Parameters.Add(new SqlParameter("@ProfesorID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.ProfesorID));
                Cm.Parameters.Add(new SqlParameter("@PredmetID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.PredmetID));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                Cm.ExecuteNonQuery();
                Cn.Close();

                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                return Ret;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 99;
            }
        }

        public static int IzmenaOcena(Ocena izmeniti)
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "dbo.OceneUPDATE";

                int Ret = 99;

                Cm.Parameters.Add(new SqlParameter("@OcenaID", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.OcenaID));
                Cm.Parameters.Add(new SqlParameter("@TipOcene", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.TipOcene));
                Cm.Parameters.Add(new SqlParameter("@Ocena", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.OcenaVrednost));
                Cm.Parameters.Add(new SqlParameter("@OpisOcene", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.OpisOcene));
                Cm.Parameters.Add(new SqlParameter("@MaticniBroj", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.MaticniBroj));
                Cm.Parameters.Add(new SqlParameter("@ProfesorID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.ProfesorID));
                Cm.Parameters.Add(new SqlParameter("@PredmetID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.PredmetID));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                Cm.ExecuteNonQuery();
                Cn.Close();

                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                return Ret;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 99;
            }
        }

        public static int BrisanjeOcena(Ocena izbrisati)
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "dbo.oceneDELETE";

                int Ret = 99;

                Cm.Parameters.Add(new SqlParameter("@OcenaID", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izbrisati.OcenaID));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                Cm.ExecuteNonQuery();
                Cn.Close();


                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                return Ret;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 99;
            }
        }

        public static PrikazOcena IzlistavanjeOcena( int BrojPoStrani, int TrenutnaStrana , string NazivPredmeta, string ImeUcenika, string ImeProfesora, int GodinaSkolovanja, int RedniBroj)
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "skola.OceneIzlistavanje";


                Cm.Parameters.Add(new SqlParameter("@BrojPoStrani", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, TrenutnaStrana));
                Cm.Parameters.Add(new SqlParameter("@TrenutnaStrana", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, TrenutnaStrana));
                Cm.Parameters.Add(new SqlParameter("@NazivPredmeta", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, NazivPredmeta));
                Cm.Parameters.Add(new SqlParameter("@ImeUcenika", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, ImeUcenika));
                Cm.Parameters.Add(new SqlParameter("@ImeProfesora", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, ImeProfesora));
                Cm.Parameters.Add(new SqlParameter("@GodinaSkolovanja", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, GodinaSkolovanja));
                Cm.Parameters.Add(new SqlParameter("@OdeljenjeBroj", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, RedniBroj));

                Cn.Open();
                PrikazOcena PO = new PrikazOcena();
                PO.Ocene = new List<Ocena>();
                SqlDataReader Dr = Cm.ExecuteReader();
                while (Dr.Read())
                {
                    PO.Ime = Dr["Ime"].ToString();
                    PO.Prezime = Dr["Prezime"].ToString();
                    PO.NazivPredmeta = Dr["NazivPredmeta"].ToString();

                    Ocena ocena = new Ocena();
                    ocena.OcenaVrednost = Convert.ToInt32(Dr["Ocena"]);
                    ocena.ImeProfesora = Dr["ImeProfesora"].ToString();
                    ocena.TipOcene = Dr["TipOcene"].ToString();
                    ocena.DatumOcene = Convert.ToDateTime(Dr["DatumOcene"]);
                    PO.Ocene.Add(ocena);
                }
                Cn.Close();
                if(PO.Ocene.Count<1){
                    return null;
                }else{
                    return PO;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        
        #endregion

        #region UceniciCRUD

        public static int DodavanjeUcenika(Ucenik dodati)
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "dbo.uceniciINSERT";

                int Ret = 99;
                Cm.Parameters.Add(new SqlParameter("@MaticniBroj", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.MaticniBroj));
                Cm.Parameters.Add(new SqlParameter("@Ime", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.Ime));
                Cm.Parameters.Add(new SqlParameter("@Prezime", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.Prezime));
                Cm.Parameters.Add(new SqlParameter("@JMBG", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.JMBG));
                Cm.Parameters.Add(new SqlParameter("@OdeljenjeID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.OdeljenjeID));
                Cm.Parameters.Add(new SqlParameter("@DatumRodjenja", SqlDbType.Date, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.DatumRodjenja));
                Cm.Parameters.Add(new SqlParameter("@MestoRodjenja", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.MestoRodjenja));
                Cm.Parameters.Add(new SqlParameter("@OpstinaRodjenja", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.OpstinaRodjenja));
                Cm.Parameters.Add(new SqlParameter("@DrzavaRodjenja", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.DrzavaRodjenja));
                Cm.Parameters.Add(new SqlParameter("@KontaktTelefonUcenika", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.KontaktTelefonUcenika));
                Cm.Parameters.Add(new SqlParameter("@EmailUcenika", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.EmailUcenika));
                Cm.Parameters.Add(new SqlParameter("@ImeOca", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.ImeOca));
                Cm.Parameters.Add(new SqlParameter("@PrezimeOca", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.PrezimeOca));
                Cm.Parameters.Add(new SqlParameter("@KontaktTelefonOca", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.KontaktTelefonOca));
                Cm.Parameters.Add(new SqlParameter("@EmailOca", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.EmailOca));
                Cm.Parameters.Add(new SqlParameter("@ImeMajke", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.ImeMajke));
                Cm.Parameters.Add(new SqlParameter("@PrezimeMajke", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.PrezimeMajke));
                Cm.Parameters.Add(new SqlParameter("@KontaktTelefonMajke", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.KontaktTelefonMajke));
                Cm.Parameters.Add(new SqlParameter("@EmailMajke", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.EmailMajke));
                Cm.Parameters.Add(new SqlParameter("@LoginSifra", SqlDbType.NVarChar, 4000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dodati.LoginSifra));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                Cm.ExecuteNonQuery();
                Cn.Close();

                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                return Ret;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 99;
            }
        }
        public static int IzmenaUcenika(Ucenik izmeniti)
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "dbo.uceniciUPDATE";

                int Ret = 99;
                Cm.Parameters.Add(new SqlParameter("@MaticniBroj", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.MaticniBroj));
                Cm.Parameters.Add(new SqlParameter("@Ime", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.Ime));
                Cm.Parameters.Add(new SqlParameter("@Prezime", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.Prezime));
                Cm.Parameters.Add(new SqlParameter("@JMBG", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.JMBG));
                Cm.Parameters.Add(new SqlParameter("@OdeljenjeID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.OdeljenjeID));
                Cm.Parameters.Add(new SqlParameter("@DatumRodjenja", SqlDbType.Date, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.DatumRodjenja));
                Cm.Parameters.Add(new SqlParameter("@MestoRodjenja", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.MestoRodjenja));
                Cm.Parameters.Add(new SqlParameter("@OpstinaRodjenja", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.OpstinaRodjenja));
                Cm.Parameters.Add(new SqlParameter("@DrzavaRodjenja", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.DrzavaRodjenja));
                Cm.Parameters.Add(new SqlParameter("@KontaktTelefonUcenika", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.KontaktTelefonUcenika));
                Cm.Parameters.Add(new SqlParameter("@EmailUcenika", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.EmailUcenika));
                Cm.Parameters.Add(new SqlParameter("@ImeOca", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.ImeOca));
                Cm.Parameters.Add(new SqlParameter("@PrezimeOca", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.PrezimeOca));
                Cm.Parameters.Add(new SqlParameter("@KontaktTelefonOca", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.KontaktTelefonOca));
                Cm.Parameters.Add(new SqlParameter("@EmailOca", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.EmailOca));
                Cm.Parameters.Add(new SqlParameter("@ImeMajke", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.ImeMajke));
                Cm.Parameters.Add(new SqlParameter("@PrezimeMajke", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.PrezimeMajke));
                Cm.Parameters.Add(new SqlParameter("@KontaktTelefonMajke", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.KontaktTelefonMajke));
                Cm.Parameters.Add(new SqlParameter("@EmailMajke", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.EmailMajke));
                Cm.Parameters.Add(new SqlParameter("@LoginSifra", SqlDbType.NVarChar, 4000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, izmeniti.LoginSifra));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                Cm.ExecuteNonQuery();
                Cn.Close();

                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;
                return Ret;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 99;
            }
        }
      
        public static List<Ucenik> IzlistavanjeUcenika(int TrenutnaStrana, int BrojPoStrani,int Razred, int RedniBroj, string NazivUcenika = "" )
        {
//             CREATE PROCEDURE skola.UceniciIzlistavanje 
// (@Razred int, @RedniBroj int, @NazivUcenika nvarchar(50))
          try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "skola.UceniciIzlistavanje ";

                int Ret = 99;

                Cm.Parameters.Add(new SqlParameter("@TrenutnaStrana", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, TrenutnaStrana));
                Cm.Parameters.Add(new SqlParameter("@BrojPoStrani", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, BrojPoStrani));
                Cm.Parameters.Add(new SqlParameter("@Razred", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Current, Razred));
                Cm.Parameters.Add(new SqlParameter("@RedniBroj", SqlDbType.Int, 4, ParameterDirection.Input,true , 0, 0, "", DataRowVersion.Current, RedniBroj));
                Cm.Parameters.Add(new SqlParameter("@NazivUcenika", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Current, NazivUcenika));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));

                Cn.Open();
                SqlDataReader dR = Cm.ExecuteReader();

                List<Ucenik> ucenici = new List<Ucenik>();

                while(dR.Read())
                {
                    Ucenik u = new Ucenik(dR.GetInt32(0),dR.GetString(1),dR.GetString(2),dR.GetString(3),dR.GetString(4),dR.GetInt32(5),dR.GetDateTime(6),dR.GetString(7),
                    dR.GetString(8),dR.GetString(9),dR.GetString(10),dR.GetString(11),dR.GetString(12),dR.GetString(13),dR.GetString(14),dR.GetString(15),dR.GetString(16),
                    dR.GetString(17),dR.GetString(18),dR.GetString(19),dR.GetString(20));

                    ucenici.Add(u);
                }

                //Console.WriteLine(ucenici[0].Ime, ucenici[0].Prezime, ucenici[0].DatumRodjenja);

                Cn.Close();


                return ucenici;

            }catch (Exception e)
            {
                Cn.Close();
                Console.WriteLine(e.Message);
                return null;
            }  

        }
        #endregion


        public static int LoginKorisnika(string korisnik, string korisnikSifra, ref int who, ref int whoID)
        {
            try
            {
                SqlCommand Cm = new SqlCommand();
                Cm.Connection = Cn;
                Cm.CommandType = CommandType.StoredProcedure;
                Cm.CommandText = "skola.LoginKorisnika";

                int Ret = 99;
                int ProfesorID =0;
                int UcenikID =0;
                bool admin = false;

                Cm.Parameters.Add(new SqlParameter("@Korisnik", SqlDbType.NVarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, korisnik));
                Cm.Parameters.Add(new SqlParameter("@LoginSifra", SqlDbType.NVarChar, 4000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, korisnikSifra));
                Cm.Parameters.Add(new SqlParameter("@ProfesorID", SqlDbType.Int, 4, ParameterDirection.Output , false, 0, 0, "", DataRowVersion.Current, ProfesorID));
                Cm.Parameters.Add(new SqlParameter("@Admin", SqlDbType.Bit, 1, ParameterDirection.Output , false, 0, 0, "", DataRowVersion.Current, admin));
                Cm.Parameters.Add(new SqlParameter("@UcenikID", SqlDbType.Int, 10, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Current, UcenikID));
                Cm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, Ret));
                
                Cn.Open();
                Cm.ExecuteNonQuery();

                ProfesorID = Cm.Parameters["@ProfesorID"].Value.GetType() == typeof(DBNull) ? ProfesorID = 0 : ProfesorID = (int)Cm.Parameters["@ProfesorID"].Value;   //(int)Cm.Parameters["@ProfesorID"].Value;
                admin = Cm.Parameters["@Admin"].Value.GetType() == typeof(DBNull) ? admin = false : admin = (bool)Cm.Parameters["@Admin"].Value;//(bool)Cm.Parameters["@Admin"].Value;
                UcenikID = Cm.Parameters["@UcenikID"].Value.GetType() == typeof(DBNull) ? UcenikID = 0 : UcenikID = (int)Cm.Parameters["@UcenikID"].Value;//(string)Cm.Parameters["@MaticniBroj"].Value;
                
                Cn.Close();

                Ret = (int)Cm.Parameters["@RETURN_VALUE"].Value;

                if(ProfesorID !=0 && admin == true) //// 0-niko , 1-ucenik , 2-profesor, 3-admin
                {
                    who = 3; whoID = ProfesorID;
                }else if (ProfesorID !=0)
                {
                    who = 2; whoID = ProfesorID;
                }else if (UcenikID !=0)
                {
                    who = 1; whoID = UcenikID;
                }else
                {
                    who = 0; whoID = 0;
                }


                return Ret;
                //Console.WriteLine($" ret {Ret}  pr {profID} admin {admin}  mb {maticniID} ");

            }
            catch (Exception ex)
            {
                Cn.Close();
                Console.WriteLine(ex.Message);
                return 99;
            }
        }
    }




    public class Profesor
    {
        
        public int ProfesorID { get; set; }
        [MinLength(2)]
        public string ImeProfesora { get; set; }

        [EmailAddress(ErrorMessage = "Neispravna imejl adresa")]
        public string Email { get; set; }
        public string KontaktTelefon { get; set; }
        public string LoginSifra { get; set; }
        public bool Admin { get; set; }

        public Profesor(){}

        public Profesor(int _ProfesorID, string _ImeProfesora, string _Email, string _KontaktTelefon, string _LoginSifra, bool _Admin)
        {
            ProfesorID = _ProfesorID;  ImeProfesora = _ImeProfesora;  Email=_Email; KontaktTelefon=_KontaktTelefon; LoginSifra=_LoginSifra; Admin=_Admin;
        }
    }

    public class Odeljenja
    {
        public int OdeljenjeID { get; set; }
        public int RedniBroj { get; set; }
        public int GodineUpisa { get; set; }
        public int Razred {get; set;}
        public int RazredniID { get; set; }
    }

    public class Ocena
    {
        public int OcenaID { get; set; }
        public string TipOcene { get; set; }
        public int OcenaVrednost { get; set; }
        public string OpisOcene { get; set; }
        public string MaticniBroj { get; set; }
        public int ProfesorID { get; set; }
        public int PredmetID { get; set; }

        public int Razred {get;set;}

        public DateTime DatumOcene { get; set; }
        public string ImeProfesora { get; set; }
    }

    public class PrikazOcena
    {
        //U.Ime, U.Prezime, P.NazivPredmeta, O.Ocena, PR.ImeProfesora, O.TipOcene, O.DatumOcene 
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string NazivPredmeta { get; set; }
        public List<Ocena> Ocene { get; set; }
    }

    public class Ucenik
    {
        public int UcenikID {get;set;}
        public string MaticniBroj { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public int OdeljenjeID { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string MestoRodjenja { get; set; }
        public string OpstinaRodjenja { get; set; }
        public string DrzavaRodjenja { get; set; }
        public string KontaktTelefonUcenika { get; set; }
        public string EmailUcenika { get; set; }
        public string ImeOca { get; set; }
        public string PrezimeOca { get; set; }
        public string KontaktTelefonOca { get; set; }
        public string EmailOca { get; set; }
        public string ImeMajke { get; set; }
        public string PrezimeMajke { get; set; }
        public string KontaktTelefonMajke { get; set; }
        public string EmailMajke { get; set; }
        public string LoginSifra { get; set; }


        public Ucenik(){}

        public Ucenik(int _UcenikID, string _MaticniBroj , string _Ime , string _Prezime , string _JMGB , int _OdeljenjeID , DateTime _DatumRodjenja,
            string _MestoRodjenja, string _OpstinaRodjenja, string _DrzavaRodjenja, string _KontaktTelefonUcenika ,string _EmailUcenika , string _ImeOca,
            string _PrezimeOca , string _KontaktTelefonOca, string _EmailOca, string _ImeMajke, string _PrezimeMajke,
            string _KontaktTelefonMajke, string _EmailMajke, string _LoginSifra)
            
        {
            UcenikID=_UcenikID; MaticniBroj=_MaticniBroj; Ime=_Ime; Prezime=_Prezime; JMBG=_JMGB; OdeljenjeID=_OdeljenjeID; DatumRodjenja=_DatumRodjenja;
            MestoRodjenja = _MestoRodjenja; OpstinaRodjenja=_OpstinaRodjenja; DrzavaRodjenja=_DrzavaRodjenja;
            KontaktTelefonUcenika=_KontaktTelefonUcenika;EmailUcenika=_EmailUcenika;
            ImeOca=_ImeOca; PrezimeOca=_PrezimeOca; KontaktTelefonOca=_KontaktTelefonOca; EmailOca=_EmailOca; ImeMajke=_ImeMajke; 
            PrezimeMajke=_PrezimeMajke; KontaktTelefonMajke=_KontaktTelefonMajke; EmailMajke=_EmailMajke; LoginSifra=_LoginSifra;
        }
    }
}