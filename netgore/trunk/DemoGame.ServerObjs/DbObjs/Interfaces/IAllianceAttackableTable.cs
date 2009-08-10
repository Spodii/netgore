using System.Linq;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `alliance_attackable`.
    /// </summary>
    public interface IAllianceAttackableTable
    {
        /// <summary>
        /// Gets the value of the database column `alliance_id`.
        /// </summary>
        AllianceID AllianceID { get; }

        /// <summary>
        /// Gets the value of the database column `attackable_id`.
        /// </summary>
        AllianceID AttackableID { get; }

        /// <summary>
        /// Gets the value of the database column `placeholder`.
        /// </summary>
        byte? Placeholder { get; }
    }
}