﻿using Database.DAO;
using System;
using System.Data.SqlClient;

namespace Utils
{
    public class DBTransaction : BaseDAO
    {
        private SqlConnection cn = null;
        private SqlTransaction tr = null;
        private bool flagTran = false;

        /// <summary>
        /// Inizia una transazione. Se c'è già una transazione in corso viene lanciata una Exception.
        /// </summary>
        public void Begin()
        {
            if (flagTran)
            {
                String msg = "Transazione già in corso";
                throw new Exception(msg);
            }
            else
            {
                cn = GetConnection();
                tr = cn.BeginTransaction();
                try
                {
                    flagTran = true;
                }
                catch (Exception ex)
                {
                    String msg = "Impossibile impostare la connessione in modalità Autocommit=false";
                    throw new Exception(msg, ex);
                }
            }
        }

        /// <summary>
        /// Esegue il commit della transazione in corso. Se viene eseguito su un istanza che non ha alcuna transazione in corso non ha effetto.
        /// </summary>
        public void Commit()
        {
            if (flagTran)
            {
                try
                {
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    String msg = "Impossibile eseguire il commit della transazione";
                    throw new Exception(msg, ex);
                }
                finally
                {
                    flagTran = false;
                    cn.Close();
                }
            }
        }

        /// <summary>
        /// Esegue il rollback della transazione in corso. Se viene eseguito su un' istanza che non ha alcuna transazione in corso non ha effetto.
        /// </summary>
        public void Rollback()
        {
            if (this.flagTran)
            {
                try
                {
                    tr.Rollback();
                }
                catch (Exception ex)
                {
                    String msg = "Impossibile eseguire il rollback della transazione";
                    throw new Exception(msg, ex);
                }
                finally
                {
                    this.flagTran = false;
                    cn.Close();
                }
            }
        }

        /// <summary>
        /// Restituisce true se c'è già una transazione in corso
        /// </summary>
        /// <returns></returns>
        public bool IsStarted()
        {
            return flagTran;
        }

        /// <summary>
        /// Restituisce l'oggetto SqlConnection nella transazione. Se viene eseguito su un istanza che non ha alcuna transazione in corso non ha effetto.
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                SqlConnection ret = null;
                if (flagTran)
                {
                    ret = cn;
                }
                return ret;
            }
        }

        /// <summary>
        /// Restituisce l'oggetto SqlTransaction nella transazione. Se viene eseguito su un istanza che non ha alcuna transazione in corso non ha effetto.
        /// </summary>
        public SqlTransaction Transaction
        {
            get
            {
                SqlTransaction ret = null;
                if (flagTran)
                {
                    ret = tr;
                }
                return ret;
            }
        }
    }
}