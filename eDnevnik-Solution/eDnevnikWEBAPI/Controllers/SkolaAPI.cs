using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eDnevnikDLL;
using SpanJson;

namespace eDnevnikWEBAPI
{
    [Route("[controller]/[action]")] 

    public class SkolaController : Controller
    {
        [HttpPost]
        public string LoginTry ( [FromBody] LoginMsg m )
        {
            if(!ModelState.IsValid)//Check if paremetars are valid
            {
                string odakle = Request.HttpContext.Connection.RemoteIpAddress.ToString();  //HttpContext.Connection.RemoteIpAddress
                Console.WriteLine($"Server primio los zahtev {odakle}");
                return "Primljen ne validan zahtev";
            }else
            {
                int whoID=0;
                int who=0;

                int passed = eDnevnikDLL.VezaSaBazom.LoginKorisnika(m.Korisnik,m.LoginSifra, ref who, ref whoID);
                
                if(passed !=0)
                {
                    return "Neuspesan login";
                }
              
                LoginMsgOut lgmo = new LoginMsgOut(who,whoID);
                var uJsonu = JsonSerializer.NonGeneric.Utf8.Serialize(lgmo);
				string converted = System.Text.Encoding.UTF8.GetString(uJsonu, 0, uJsonu.Length);//byte to string
				return converted;
            }

        }



        [HttpPost]
        public string AdminProfesoriIzlistaj ( [FromBody] SelectMsg m )
        {
            if(!ModelState.IsValid)//Check if paremetars are valid
            {
                return "Pls send request in corect form :)";
            }else
            {
                List<Profesor> profesori= eDnevnikDLL.VezaSaBazom.IzlistavanjeProfesora();
                var uJsonu = JsonSerializer.NonGeneric.Utf8.Serialize(profesori);
                if(uJsonu==null)
                {
                    Console.WriteLine("json je null");
                    return "cant process empty request";
                }

				string converted = System.Text.Encoding.UTF8.GetString(uJsonu, 0, uJsonu.Length);//byte to string
				return converted;
            }
        }


        [HttpPost]
        public string AdminProfesoriDodaj ( [FromBody] Profesor m )
        {
            if(!ModelState.IsValid)//Check if paremetars are valid
            {
                string badResp = "";

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        //Console.WriteLine(error.ErrorMessage.ToString());
                        badResp = badResp + error.ErrorMessage.ToString();
                    }
                }
                if(badResp.Length<5)
                {
                    return badResp;
                }else{
                    return "Hmm nesto ne valja!";
                }
            }else
            {
                if(eDnevnikDLL.VezaSaBazom.DodavanjeProfesora(m) == 0)
                {
                    return "OK";
                }else
                {
                    return "NE";
                }
            }
        }






        [HttpPost] //AUtorize role ucenik
        public string AdminUceniciIzlistaj ( [FromBody] SelectMsg m )
        {
            if(!ModelState.IsValid)//Check if paremetars are valid
            {
                Console.WriteLine("aaaaa");
                return "Pls send request in corect form :)";
            }else
            {
                List<Ucenik> ucenici = eDnevnikDLL.VezaSaBazom.IzlistavanjeUcenika( m.TrenutnaStrana, m.BrojPoStrani , m.Razred, m.RedniBroj , m.NazivUcenika);
                var uJsonu = JsonSerializer.NonGeneric.Utf8.Serialize(ucenici);
                if(uJsonu==null)
                {
                    Console.WriteLine("json je null");
                    return "cant process empty request";
                }
                //Console.WriteLine(uJsonu.Length);

				string converted = System.Text.Encoding.UTF8.GetString(uJsonu, 0, uJsonu.Length);//byte to string
				return converted;
            }
        }


        [HttpPost] //AUtorize role ucenik
        public string PrikazUceniku ( [FromBody] LoginMsg m )
        {
            if(!ModelState.IsValid)//Check if paremetars are valid
            {
                Console.WriteLine("aaaaa");
                return "Pls send request in corect form :)";
            }else
            {
                return "aaa";
            }
        }
    }

/* 
    [Route("[controller]/[action]")] 
    public class PhoneApiController : Controller
    {

        [HttpPost]
        public string ListByPage ( [FromBody] Msg m )
        {
            if(!ModelState.IsValid)//Check if paremetars are valid
            {
                return "Pls send request in corect form :)";
            }else
            {
                //return PhoneDB.ListByPage( m.pageNum, m.nPerPage , m.lastName ); 
            }

        }

        [HttpPost]
        public string AddNewEntry([FromBody] PhoneEntry pe)
        {
            if (!ModelState.IsValid)
            {
                return "Pls send request in corect form :)";
            }
            else
            {
                //return PhoneDB.AddNewEntry(pe.FirstName,pe.LastName, pe.TelephoneNumber);
            }
        }

        [HttpPost]
        public string DeleteRecord([FromBody] int? recordID)
        {
            if(!ModelState.IsValid)
            {
                return "Pls send request in corect form :)";
            }else
            {
                //return PhoneDB.DeleteRecord((int)recordID);
            }
        }
    }
*/

    public class Msg //Helper class , i used it just to envade having to use GET and querys when i allready done evrything in POST :)
    {
        [Required]
        public int pageNum {get; set;}
        [Required]
        public int nPerPage {get; set;}
        [Required(AllowEmptyStrings=true)]
        public string lastName {get; set;}
    }


#region  KlaseZaLogin
    public class LoginMsg
    {
        [Required]
        public string Korisnik{get;set;}
        [Required, MinLength(8)]
        public string LoginSifra{get;set;}
    }
    public class LoginMsgOut
    {
        public int who {get;set;} // 0-niko , 1-ucenik , 2-profesor, 3-admin
        public int ID { get; set; }

        public LoginMsgOut(){}
        public LoginMsgOut(int _who, int _ID)
        {
            who= _who; ID = _ID;
        }
    }
#endregion


    public class SelectMsg
    {
        public int TrenutnaStrana {get;set;}
        public int BrojPoStrani {get;set;}

        public string NazivPredmeta {get;set;}
        //[Required(AllowEmptyStrings=true)]
        public string NazivUcenika {get;set;}
        public string NazivProfesora {get;set;}
        public int Razred {get;set;} 
        public int RedniBroj {get;set;}

        public int GodinaUpisa {get;set;}
    }
}

