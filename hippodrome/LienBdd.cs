using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

// Slagevuk!
namespace hippodrome
{
    class LienBdd
    {
        // fabien ne sert a rien
        // propriétés membres
        //
        // test modif
        private SqlConnection cn;
        private SqlCommand cde;
        private SqlDataAdapter da;
        private DataTable dt;
        //
        // méthodes
        // couscous
        /// <summary>
        /// constructeur de la connexio
        /// </summary>
        public LienBdd()
        {
            try
            {
                // on commence par récupérer dans ch les informations contenues dans le fichier app.config
                // pour la connectionString de nom MaConnection
                string ch = ConfigurationManager.ConnectionStrings["MaConnection"].ConnectionString;
                cn = new SqlConnection(ch);
                cn.Open();
            }
            catch (SqlException)
            {
                throw new Exception("Erreur à la connexion");
            }
        }
        /// <summary>
        /// Méthode permettant de fermer la connexion
        /// </summary>
        public void FermerConnexion()
        {
            this.cn.Close();
        }
        ///// <summary>
        ///// permet de récupérer le contenu de la table course. 
        ///// </summary>
        ///// <returns>un objet de type datatable contenant les données récupérées</returns>
        public DataTable ObtenirCourses()
        {
            String req = "select * from course";
            this.cde = new SqlCommand(req, cn);
            da = new SqlDataAdapter();
            da.SelectCommand = this.cde;
            dt = new DataTable();
            //Le DataAdapter da va se charger ensuite de remplir la DataTable 
            da.Fill(dt);

            return dt;
        }
        ///// <summary>
        ///// permet de mettre à jour  la table course  
        ///// </summary>
        public void MajCourse(int i, string pHippodrome, DateTime pDate, int pDistance)
        {
            // on remplit la propriété dt (contenant la Data Table concernée)  avec les champs textbox (sauf l'id bien sûr!!!)
            dt.Rows[i]["hippodrome"] = pHippodrome;
            dt.Rows[i]["date"] = pDate;
            dt.Rows[i]["distance"] = pDistance;
            // on utilise le command builder qui a généré automatiquement les ordres insert, delete et update d'après le contenu du da
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);
            commandBuilder.GetUpdateCommand();
            try
            {
                da.Update(dt);  // écrit dans la base de données
                dt.AcceptChanges(); // 

            }
            catch (Exception ex)
            {
                dt.RejectChanges();  // on annule l ajout dans la dataTable car il s'est mal passé
                throw new Exception(" Erreur suppression SqlServeur  \n" + ex.Message);
            }
        }

        public DataTable ObtenirCheval()
        {
            String req = "select * from cheval order by nomche";
            this.cde = new SqlCommand(req, cn);
            da = new SqlDataAdapter();
            da.SelectCommand = this.cde;
            dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable ObtenirResultatCheval(string Pid)
        {
            String req = "select nomche, place, course.id from cheval, participe, course where cheval.id = idche and idcourse = course.id and idche = @unidcheval";
            this.cde = new SqlCommand(req, cn);
            this.cde.Parameters.Add("@unidcheval", SqlDbType.VarChar).Value = Pid;
            da = new SqlDataAdapter();
            da.SelectCommand = this.cde;
            dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}
     
    
        

