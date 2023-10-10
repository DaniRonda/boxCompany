using Dapper;
using Npgsql;

namespace infrastructure.Repositories;

public class BoxRepository
{
    private NpgsqlDataSource _dataSource;
    
    public BoxRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }

    public IEnumerable<BoxQuery> GetBoxFeed()
    {
        string sql = $@"
SELECT box_id as {nameof(BoxQuery.BoxId)},
       box_name as {nameof(BoxQuery.BoxName)},
        box_description as {nameof(BoxQuery.BoxDescription)},
        box_img_url as {nameof(BoxQuery.BoxImgUrl)}
FROM public.boxes;
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<BoxQuery>(sql);
        }
    }


}

public class BoxQuery
{
    public int BoxId { get; set; }
    public string? BoxName { get; set; }
    public string? BoxImgUrl { get; set; }
    public string? BoxDescription { get; set; }
}