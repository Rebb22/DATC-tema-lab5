using Microsoft.WindowsAzure.Storage.Table;



//clasa asta contine campurile din inregistrare

namespace DATC_tema_lab4
{

public class StudentEntity : TableEntity
{
    public string FirstName{get;set;}
    public string LastName{get;set;}

    public string Email{get;set;}

    public string PhoneNumber{get;set;}

    public int YEar{get;set;}

    public string Faculty{get;set;}


    public StudentEntity(string university, string cnp)
    {
        this.PartitionKey=university;
        this.RowKey=cnp;
    }

    public StudentEntity()
    {

    }



}



}