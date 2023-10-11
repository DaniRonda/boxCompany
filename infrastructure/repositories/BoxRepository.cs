using Dapper;
using infrastructure.DataModels;
using infrastructure.QueryModels;
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
    public Box CreateBox(string boxName, string boxDescription, string boxImgUrl)
    {
        var sql = $@"
INSERT INTO public.boxes (box_name, box_description, box_img_url) 
VALUES (@boxName, @boxDescription, @boxImgUrl)
RETURNING box_id as {nameof(Box.BoxId)},
       box_name as {nameof(Box.BoxName)},
        box_description as {nameof(Box.BoxDescription)},
        box_img_url as {nameof(Box.BoxImgUrl)},
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Box>(sql, new {boxName, boxDescription, boxImgUrl });
        }
    }
    
    
    public bool DeleteBox(int boxId)
    {
        var sql = @"DELETE FROM public.boxes WHERE box_id = @boxId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { boxId }) == 1;
        }
    }
    
    public Box UpdateBox(string boxName, int boxId, string boxDescription, string boxImgUrl)
    {
        var sql = $@"
UPDATE public.boxes SET box_name = @boxName, box_description = @boxDescription, box_img_url = @boxImgUrl
WHERE box_id = @boxId
RETURNING box_id as {nameof(Box.BoxId)},
       box_name as {nameof(Box.BoxName)},
        box_description as {nameof(Box.BoxDescription)},
        box_img_url as {nameof(Box.BoxImgUrl)};";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Box>(sql, new { boxName, boxId, boxDescription, boxImgUrl});
        }
    }
    

    public bool DoesNameofTheBoxExist(string boxName)
    {
        var sql = @"SELECT COUNT(*) FROM public.boxes WHERE box_name = @boxName;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.ExecuteScalar<int>(sql, new { boxName }) == 1;
        }
    }

}
