namespace infrastructure;

public class Utilities
{
    private static readonly Uri Uri = new Uri("postgres://ugcxjvev:v6lkeLTgB15DtqPizyzU0mgEeibsiD_h@cornelius.db.elephantsql.com/ugcxjvev");
    
    public static readonly string
        ProperlyFormattedConnectionString = string.Format(
            "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
            Uri.Host,
            Uri.AbsolutePath.Trim('/'),
            Uri.UserInfo.Split(':')[0],
            Uri.UserInfo.Split(':')[1],
            Uri.Port > 0 ? Uri.Port : 5432);

}
