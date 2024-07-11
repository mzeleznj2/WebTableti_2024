using DevExpress.Emf;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.ServiceModel.MsmqIntegration;
using System.Web;
using System.Web.DynamicData;
using System.Web.Helpers;
using System.Web.Mvc;

namespace WebTableti.Models
{
    public class Podaci
    {
        string connString = ConfigurationManager.ConnectionStrings["dbNautilusConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();        
        List<PodaciDetalji> sviPodaci = new List<PodaciDetalji>();
        List<PodaciDetalji> sviPodaci06 = new List<PodaciDetalji>();
        List<EmailCampionario> emailCamp = new List<EmailCampionario>();


        public DataTable NapuniGrid()
        {
            DataTable dtPodaci = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                string email = "grupa1.nautilus@tubla.local";
                conn.Open();
                cmd = new SqlCommand("Select * from NF_Tracc_fermi_Grupa33_NEW where Email=@email and LastStopCode in (50002,50007,50008) order by Email, DateRec desc", conn);
                cmd.Parameters.AddWithValue("@email", email);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPodaci);
            }        
           
            return dtPodaci;
        }

      
        public DataTable NapuniGrid2()
        {
            DataTable dtPodaci = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                string email = "grupa2.nautilus@tubla.local";
                conn.Open();
                cmd = new SqlCommand("Select * from NF_Tracc_fermi_Grupa33_NEW where Email=@email and LastStopCode in (50002,50007,50008) order by Email, DateRec desc", conn);
                cmd.Parameters.AddWithValue("@email", email);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPodaci);
            }

            return dtPodaci;
        }


        public DataTable NapuniGrid3()
        {
            DataTable dtPodaci = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                string email = "grupa3.nautilus@tubla.local";
                conn.Open();
                cmd = new SqlCommand("Select * from NF_Tracc_fermi_Grupa33_NEW where Email=@email and LastStopCode in (50002,50007,50008) order by Email, DateRec desc", conn);
                cmd.Parameters.AddWithValue("@email", email);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPodaci);
            }

            return dtPodaci;
        }


        //Za prikaz 2 modela na View Details.cshtml
        public List<PodaciDetalji> sviDetalji(int kod ) 
        {
            DataTable dtPovijest = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                cmd = new SqlCommand("Select * from NF_Tracc_fermi_96ore where MachCode = @code order by DateRec desc", conn);
                cmd.Parameters.AddWithValue("@code", kod);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPovijest);
            }

            foreach (DataRow pod in dtPovijest.Rows)
            {
                PodaciDetalji detaljiSve = new PodaciDetalji();

                detaljiSve.datum = Convert.ToDateTime(pod["DateRec"]);
                detaljiSve.kodMasine = Convert.ToInt32(pod["MachCode"]);
                detaljiSve.stopKod = Convert.ToInt32(pod["StopCode"]);
                detaljiSve.poruka = pod["Text"].ToString();
                detaljiSve.User = pod["UserCode"].ToString();
                detaljiSve.Shift = Convert.ToInt32(pod["Shift"]);

                sviPodaci.Add(detaljiSve);            
            }
           
            return sviPodaci;
            
        }


        public List<PodaciDetalji> sviDetalji06(string linija)
        {
            DataTable dtPovijest06 = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                cmd = new SqlCommand("select * from MZD_Tracc_fermi_50006_120ore where RoomCode = @linija order by DateRec", conn);
                cmd.Parameters.AddWithValue("@linija", linija);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPovijest06);
            }

            DataTable dtPovijest0600 = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                cmd = new SqlCommand("select * from MZD_Tracc_Fermi_nula50006_120ore where RoomCode = @linija and StopCode = 0 order by DateRec", conn);
                cmd.Parameters.AddWithValue("@linija", linija);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPovijest0600);
            }



            foreach (DataRow pod06 in dtPovijest06.Rows)
            {
                PodaciDetalji detaljiSve06 = new PodaciDetalji();
                int brojac = 0;

                detaljiSve06.datum = Convert.ToDateTime(pod06["DateRec"]);
                detaljiSve06.kodMasine = Convert.ToInt32(pod06["MachCode"]);
                detaljiSve06.stopKod = Convert.ToInt32(pod06["StopCode"]);
                detaljiSve06.poruka = pod06["Text"].ToString();
                detaljiSve06.User = pod06["UserCode"].ToString();
                sviPodaci06.Add(detaljiSve06);

                foreach (DataRow pod0600 in dtPovijest0600.Rows)
                {                    

                    if (brojac == 0 && Convert.ToInt32(pod06["MachCode"]) == Convert.ToInt32(pod0600["MachCode"]) &&
                             pod06["StyleCode"].ToString() == pod0600["StyleCode"].ToString() &&
                             Convert.ToDateTime(pod06["DateRec"]) < Convert.ToDateTime(pod0600["DateRec"]))
                    {                     
                        PodaciDetalji detaljiSve00 = new PodaciDetalji();

                        detaljiSve00.datum = Convert.ToDateTime(pod0600["DateRec"]);
                        detaljiSve00.kodMasine = Convert.ToInt32(pod0600["MachCode"]);
                        detaljiSve00.stopKod = Convert.ToInt32(pod0600["StopCode"]);
                        detaljiSve00.poruka = pod0600["Text"].ToString();
                        detaljiSve00.User = pod0600["UserCode"].ToString();

                        sviPodaci06.Add(detaljiSve00);
                        brojac++;
                    }
                }

               
            }

            
            return sviPodaci06;

        }

        //------------------------------------------------------------------------------------

        public DataTable NapuniGridPoLinijiGrupa1(int linija)
        {
            string Linija = ("LINEA-" + linija).ToString();

            DataTable dtPodaci = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                string email = "grupa1.nautilus@tubla.local";
                conn.Open();
                cmd = new SqlCommand("Select * from NF_Tracc_fermi_Grupa33_NEW where Email=@email and LastStopCode in (50002,50007,50008) and RoomCode=@linija order by Email, DateRec desc", conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@linija", Linija);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPodaci);
            }

            return dtPodaci;
        }

        public DataTable NapuniGridPoLinijiGrupa2(int linija)
        {
            string Linija = ("LINEA-" + linija).ToString();

            DataTable dtPodaci = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                string email = "grupa2.nautilus@tubla.local";
                conn.Open();
                cmd = new SqlCommand("Select * from NF_Tracc_fermi_Grupa33_NEW where Email=@email and LastStopCode in (50002,50007,50008) and RoomCode=@Linija order by Email, DateRec desc", conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@linija", Linija);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPodaci);
            }

            return dtPodaci;
        }


        public DataTable NapuniGridPoLinijiGrupa3(int linija)
        {
            string Linija = ("LINEA-" + linija).ToString();

            DataTable dtPodaci = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                string email = "grupa3.nautilus@tubla.local";
                conn.Open();
                cmd = new SqlCommand("Select * from NF_Tracc_fermi_Grupa33_NEW where Email=@email and LastStopCode in (50002,50007,50008) and RoomCode=@Linija order by Email, DateRec desc", conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@linija", Linija);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPodaci);
            }

            return dtPodaci;
        }

        //public DataTable NapuniGrupu3()
        //{
        //    DataTable dtPodaci = new DataTable();
        //    using (conn = new SqlConnection(connString))
        //    {
        //        conn.Open();
        //        cmd = new SqlCommand("select * from NF_Tracc_fermi_Grupa33 where StopCode in (50002,50007,50008)", conn);
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        adapter.Fill(dtPodaci);
        //    }          

        //    return dtPodaci;
        //}

        public int vratiSumuBrojac2(string email)
        {
            int brojac;
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                cmd = new SqlCommand("select count(*) from NF_Tracc_fermi_Grupa3 where LastStopCode = 50002 and Email=@email", conn);
                cmd.Parameters.AddWithValue("@email", email);

                brojac = (int)cmd.ExecuteScalar();
            }
            return brojac;
        }

        public int vratiSumuBrojac7(string email)
        {
            int brojac;
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                cmd = new SqlCommand("select count(*) from NF_Tracc_fermi_Grupa3 where LastStopCode = 50007 and Email=@email", conn);
                cmd.Parameters.AddWithValue("@email", email);

                brojac = (int)cmd.ExecuteScalar();
            }
            return brojac;
        }

        public int vratiSumuBrojac8(string email)
        {
            int brojac;
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                cmd = new SqlCommand("select count(*) from NF_Tracc_fermi_Grupa3 where LastStopCode = 50008 and Email=@email", conn);
                cmd.Parameters.AddWithValue("@email", email);

                brojac = (int)cmd.ExecuteScalar();
            }
            return brojac;
        }


        // Brojač na drugi način

        public DataTable brojacGrupa1()
        {
            DataTable dtPodaciBrojac = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                cmd = new SqlCommand("select LastStopCode, count(LastStopCode) as Brojac " +
                    "from NF_Tracc_fermi_Grupa33_NEW where Email='grupa1.nautilus@tubla.local' group by LastStopCode", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPodaciBrojac);
            }
            
            return dtPodaciBrojac;
        }

        public DataTable brojacGrupa2()
        {
            DataTable dtPodaciBrojac = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                cmd = new SqlCommand("select LastStopCode, count(LastStopCode) as Brojac " +
                    "from NF_Tracc_fermi_Grupa33_NEW where Email='grupa2.nautilus@tubla.local' group by LastStopCode", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPodaciBrojac);
            }

            return dtPodaciBrojac;
        }


        public DataTable brojacGrupa3()
        {
            DataTable dtPodaciBrojac = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                cmd = new SqlCommand("select LastStopCode, count(LastStopCode) as Brojac " +
                    "from NF_Tracc_fermi_Grupa33_NEW where Email='grupa3.nautilus@tubla.local' group by LastStopCode", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPodaciBrojac);
            }

            return dtPodaciBrojac;
        }

        //

        public void AzurirajMasine()
        {
            DataTable dtMasine = new DataTable();
            using (conn = new SqlConnection(connString))
            {
               cmd = new SqlCommand("select MachCode, MachineId from MACHINES_SETUP with (NOLOCK)", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtMasine);
            }

            DataTable dtRemont = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                cmd = new SqlCommand("select MachCode, MachineId from RemontMach with (NOLOCK)", conn);
                SqlDataAdapter adapter2 = new SqlDataAdapter(cmd);
                adapter2.Fill(dtRemont);
            }

            foreach (DataRow drRem in dtRemont.Rows)
            {
                foreach (DataRow drMas in dtMasine.Rows)
                {
                    if ((drRem["MachineId"] == drMas["MachineId"]) && (drRem["MachCode"] != drMas["MachCode"]) )
                    {
                        using (conn = new SqlConnection(connString))
                        {
                            cmd = new SqlCommand("UPDATE RemontMach SET MachCode = @brojMasine where MachineId=@matricola", conn);

                            cmd.Parameters.AddWithValue("@brojMasine", drMas["MachCode"]);
                            cmd.Parameters.AddWithValue("@matricola", drRem["MachineId"]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

     

        public void SendEmailToCampionario()
        {
            DataTable dtPodaciZaEmail = new DataTable();
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                cmd = new SqlCommand("Select * from TUBLA_CAMPIONARIO_EMAIL where EmailStatus = 0 ", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtPodaciZaEmail);
            }

            foreach (DataRow pod in dtPodaciZaEmail.Rows)
            {
                EmailCampionario email = new EmailCampionario();
                
                email.StopDate  = Convert.ToDateTime(pod["StopDate"]) ;
                email.RoomCode  = pod["RoomCode"].ToString();
                email.GroupCode = pod["GroupCode"].ToString();
                email.MachCode = Convert.ToInt32(pod["MachCode"]);
                email.UniqueId = pod["UniqueId"].ToString();               
                email.UserCode = pod["UserCode"].ToString() is null ? "" : pod["UserCode"].ToString();
                email.Text = pod["Text"].ToString() is null? "" : pod["Text"].ToString();
                email.EmailStatus = Convert.ToInt32(pod["EmailStatus"]);

                emailCamp.Add(email);
            }
         
            //prođi kroz listu i dodaj elemnte u Email
            foreach (var podatak in emailCamp)
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("Admin", "admin@tubla.hr"));
                email.To.Add(new MailboxAddress("Nikolina", "nikolina.sovar@tubla.hr"));
                email.To.Add(new MailboxAddress("Campionario", "campionario@tubla.hr"));
                email.To.Add(new MailboxAddress("Zaklina", "zaklina.sedlarevic@tubla.hr"));
                email.To.Add(new MailboxAddress("Mehanicari", "meccanici.smacchinatura@tubla.hr"));               
                email.Cc.Add(new MailboxAddress("Maja", "maja.zeleznjak@tubla.hr"));


                //naslov : FERMO CAMPIONARIO
                email.Subject = "FERMO CAMPIONARIO";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = "Machine: " + podatak.MachCode + "\n" +
                           "StopDate: " + podatak.StopDate + "\n" +
                           "User: " + podatak.UserCode + "\n" +
                           "Text: " + podatak.Text + ""
                };               

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("mail.a1net.hr", 25, MailKit.Security.SecureSocketOptions.None);                   

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }

                using (conn = new SqlConnection(connString))
                {
                    conn.Open();
                    cmd = new SqlCommand("UPDATE TUBLA_CAMPIONARIO_EMAIL SET EmailStatus = 1 where UniqueId = @uniqueId ", conn);
                    cmd.Parameters.AddWithValue("@uniqueId", podatak.UniqueId);
                    cmd.ExecuteNonQuery();
                }

            }

        }


  

    }

}