
//namespace StudentApi.Models
//{
//    public static class CollegeRepository

//    {
//        public static List<Student> Students { get; set; } = new List<Student>(){
//            new Student
//            {
//                Id = 1,
//                Name = "Test1",
//                Email = "abc@gmail.com",
//                Address = "abc"
//            },
//            new Student
//            {
//                Id = 2,
//                Name = "Test2",
//                Email = "abc@gmail.com",
//                Address = "abc"

//            }
//            };

//        internal static IEnumerable<object> Students()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}


namespace StudentApi.Models
{
    public static class CollegeRepository
    {
        // This should be a property, not a method
        public static List<Student> Students { get; set; } = new List<Student>()
        {
            new Student
            {
                Id = 1,
                Name = "Test1",
                Email = "abc@gmail.com",
                Address = "abc"
            },
            new Student
            {
                Id = 2,
                Name = "Test2",
                Email = "abc@gmail.com",
                Address = "abc"
            }
        };
    }
}
