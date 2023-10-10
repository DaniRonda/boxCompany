using infrastructure.Repositories;

namespace service;

public class BoxService
{

    private readonly BoxRepository _boxRepository;
    
    public BoxService(BoxRepository boxRepository)
    {
        _boxRepository = boxRepository;
    }

    public IEnumerable<BoxQuery> GetBoxFeed()
    {
        return _boxRepository.GetBoxFeed();
    }

}
