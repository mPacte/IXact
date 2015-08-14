using System;
using mPacte.Orm.DAL;

namespace mPacte.Orm
{
    public class OrmManager
    {
        public AbstractDAL DAL { get; set; }

        private static OrmManager _currentSession;

        public static OrmManager CurrentSession
        {
            get
            {
                if (_currentSession == null)
                    _currentSession = new OrmManager();
                return _currentSession;
            }
        }

        public Criteria NewCriteria(String Name, Object Value)
        {
            var criteria = new Criteria();
            criteria.Name = Name;
            criteria.Value = Value;
            return criteria;
        }
    }
}