using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Transactions;
using IsolationLevel=System.Transactions.IsolationLevel;

namespace MySql.Data.MySqlClient
{
    sealed class MySqlPromotableTransaction : IPromotableSinglePhaseNotification, ITransactionPromoter
    {
        readonly Transaction baseTransaction;
        readonly MySqlConnection connection;
        MySqlTransaction simpleTransaction;

        public Transaction BaseTransaction
        {
            get { return baseTransaction; }
        }

        public MySqlPromotableTransaction(MySqlConnection connection, Transaction baseTransaction)
        {
            this.connection = connection;
            this.baseTransaction = baseTransaction;
        }

        #region IPromotableSinglePhaseNotification Members

        void IPromotableSinglePhaseNotification.Initialize()
        {
            string valueName = Enum.GetName(typeof(IsolationLevel), baseTransaction.IsolationLevel);
            System.Data.IsolationLevel dataLevel =
                (System.Data.IsolationLevel)Enum.Parse(typeof(System.Data.IsolationLevel), valueName);

            simpleTransaction = connection.BeginTransaction(dataLevel);
        }

        void IPromotableSinglePhaseNotification.Rollback(SinglePhaseEnlistment singlePhaseEnlistment)
        {
            simpleTransaction.Rollback();
            singlePhaseEnlistment.Aborted();
            DriverTransactionManager.RemoveDriverInTransaction(baseTransaction);

            connection.driver.CurrentTransaction = null;

            if (connection.State == ConnectionState.Closed)
                connection.CloseFully();
        }

        void IPromotableSinglePhaseNotification.SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment)
        {
            simpleTransaction.Commit();
            singlePhaseEnlistment.Committed();
            DriverTransactionManager.RemoveDriverInTransaction(baseTransaction);

            connection.driver.CurrentTransaction = null;

            if (connection.State == ConnectionState.Closed)
                connection.CloseFully();
        }

        byte[] ITransactionPromoter.Promote()
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    class DriverTransactionManager
    {
        static readonly Hashtable driversInUse = new Hashtable();

        public static Driver GetDriverInTransaction(Transaction transaction)
        {
            lock (driversInUse.SyncRoot)
            {
                Driver d = (Driver)driversInUse[transaction.GetHashCode()];
                return d;
            }
        }

        public static void RemoveDriverInTransaction(Transaction transaction)
        {
            lock (driversInUse.SyncRoot)
            {
                driversInUse.Remove(transaction.GetHashCode());
            }
        }

        public static void SetDriverInTransaction(Driver driver)
        {
            lock (driversInUse.SyncRoot)
            {
                driversInUse[driver.CurrentTransaction.BaseTransaction.GetHashCode()] = driver;
            }
        }
    }
}