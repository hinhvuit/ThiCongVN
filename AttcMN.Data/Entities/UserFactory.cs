using SqlSugar;

namespace AttcMN.Data.Entities
{
    [SugarTable("user_factory", "User And Factory")]
    public class UserFactory: BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [SugarColumn(ColumnName = "user_id", ColumnDescription = "userId")]
        public long UserId { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the factory.
        /// </summary>
        [SugarColumn(ColumnName = "fac_id", ColumnDescription = "Factory Id")]
        public int FacId { get; set; }
    }
}
