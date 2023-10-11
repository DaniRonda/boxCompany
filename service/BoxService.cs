using System.ComponentModel.DataAnnotations;
using infrastructure.DataModels;
using infrastructure.QueryModels;
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

    public Box CreateBox(string boxName, string boxDescription, string boxImgUrl)
    {
        var doesBoxExist = _boxRepository.DoesNameofTheBoxExist( boxName);
        if (!doesBoxExist)
        {
            throw new ValidationException("The box with the name " + boxName + "already exists");
        }
        
        return _boxRepository.CreateBox(boxName, boxDescription, boxImgUrl);
    }

    public Box UpdateBox(string boxName, int boxId, string boxDescription, string boxImgUrl)
    {
       return _boxRepository.UpdateBox(boxName, boxId, boxDescription, boxImgUrl);
    }
    public void DeleteBox(int boxId)
    {
        
        var result = _boxRepository.DeleteBox(boxId);
        if (!result)
        {
            throw new Exception("Box could not be deleted");
        }
    }
}