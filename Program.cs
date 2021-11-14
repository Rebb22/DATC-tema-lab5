using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


namespace DATC_tema_lab4
{
    
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        static void Main(string[] args)
        {
            Task.Run(async () => { await Initialize(); })     //legatura cu azure storage
            .GetAwaiter()
            .GetResult();
        }

        static async  Task Initialize()
             {
                string storageConnectionString="DefaultEndpointsProtocol=https;"
                 + "AccountName=azurestoragerebecca"
                 + ";AccountKey=/tursTPcvGp7MhSfK9Aywaq38DnD81s5Pmk7FPZOZg99d/pHsJOkL86AU3VkxzPpwTk8bedKQ7GgnQu2nvQiWA=="
                 + ";EndpointSuffix=core.windows.net";

                var account=CloudStorageAccount.Parse(storageConnectionString);
                tableClient=account.CreateCloudTableClient();

                studentsTable=tableClient.GetTableReference("Studenti");   //referinta catre tabelul studenti

                await studentsTable.CreateIfNotExistsAsync();              //creaza tabelul daca nu exista

                await EditStudent();
                // await AddNewStudent();
                //await GetAllStudents();

            }

            private static async Task GetAllStudents()
            {
                Console.WriteLine("University\tCNP\tNAme\tNume\tEmail\tNtTelefon\tAn");
                TableQuery<StudentEntity>query = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey",QueryComparisons.Equal,"UPT"));
               //.Where(TableQuery.GenerateFilterCondition("university",QueryComparisons.Equal,"cnp"));
                TableContinuationToken token =null;
                do
                {
                    TableQuerySegment<StudentEntity> resultSegment=await studentsTable.ExecuteQuerySegmentedAsync(query,token);
                    token=resultSegment.ContinuationToken;

                    foreach(StudentEntity entity in resultSegment.Results)
                    {
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",entity.PartitionKey,entity.RowKey,entity.FirstName,entity.LastName,entity.Email,entity.PhoneNumber,entity.YEar);
                    }
                }while(token!=null);
            }


            private static async Task AddNewStudent()     //se creeaza o inregistrare in tabel
            {
                var student = new StudentEntity("UPT", "224587851455");
                student.FirstName="Potocianu";
                student.LastName="Rebecca";
                student.Email="rebecca94potocianu@yahoo.com";
                student.YEar=4;
                student.PhoneNumber="0747589621";
                student.Faculty="AC";

                var insertOperation=TableOperation.Insert(student);

                await studentsTable.ExecuteAsync(insertOperation);

            }

            public static async Task EditStudent()
            {
                var student = new StudentEntity("UPT","5482145226");
                student.FirstName="Dave";
                student.YEar=4;
                student.ETag="*";
                var editOperation=TableOperation.Merge(student);

                await studentsTable.ExecuteAsync(editOperation);
            }



            
    }




}
