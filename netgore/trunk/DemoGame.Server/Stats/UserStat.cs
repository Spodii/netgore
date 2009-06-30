using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Server
{
    class UserStat<T> : Stat<T>, IUserStat, IUpdateableStat where T : IStatValueType, new()
    {
        readonly T _lastUpdatedValue = new T();
        readonly User _user;
        StatUpdateHandler _updateHandler;

        public UserStat(User user, StatUpdateHandler updateHandler, StatType statType) : base(statType)
        {
            _user = user;
            UpdateHandler = updateHandler;
        }

        #region IUpdateableStat Members

        public int LastUpdatedValue
        {
            get { return _lastUpdatedValue.GetValue(); }
        }

        public StatUpdateHandler UpdateHandler
        {
            get { return _updateHandler; }
            set { _updateHandler = value; }
        }

        public virtual bool NeedsUpdate
        {
            get { return _lastUpdatedValue.GetValue() != Value; }
        }

        public virtual void Update()
        {
            if (!NeedsUpdate || UpdateHandler == null)
                return;

            UpdateHandler(this);
            _lastUpdatedValue.SetValue(Value);
        }

        #endregion

        #region IUserStat Members

        public User User
        {
            get { return _user; }
        }

        #endregion
    }
}