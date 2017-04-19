using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [GlobalErrorBehaviorAttribute(typeof(GlobalErrorHandler))]
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            if (value == 99)
            {
                // just throwing exception.
                throw new Exception("Method will not accept 99");
            }
            try
            {
                Enumerable.Range(1, 100).Contains(value);
                Convert.ToDateTime(value);
            }
            catch (Exception ex)
            {
                var e = ex.Message;
                Console.WriteLine(e);

                throw new Exception(ex.Message);
            }

            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public int Divide(int num1, int num2)
        {
			var a = num1 / num2;
			return a;
        }
		
		public void ServiceErrorTest(int count)
		{
			switch (count)
			{
				case 1: //Case1: Attempt to dividy by zero     - System.DivideByZeroException - Handles errors generated from dividing a dividend with zero.
					var a = 4;
					var b = 0;
					var z = a / b;                 // <-- Causes exception
					break;
				case 2: //Case2: raises NullReferenceException - System.NullReferenceException - Handles errors generated from deferencing a null object.
					string value = null;
					if (value.Length == 0)        // <-- Causes exception
					{
						Console.WriteLine(value);
					}
					break;
				case 3: //Case3: Session expired
					System.Web.HttpContext.Current.Session.Clear();// <-- Causes exception
					break;
				case 4:  //Case4: System.IO.IOException - Handles I/O errors.
					System.IO.FileStream F = new System.IO.FileStream("sample.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read); // <-- Causes exception
					break;
				case 5: //Case5: accessses array out of bounds
						// System.IndexOutOfRangeException - Handles errors generated when a method refers to an array index out of range.
					int[] array = new int[100];
					array[0] = 1;
					array[10] = 2;
					array[200] = 3; // <-- Causes exception
					break;
				case 6: //Case6: Attempted to access an element as a type incompatible with the array. 
						//System.ArrayTypeMismatchException - Handles errors generated when type is mismatched with the array type.
					string[] array1 = { "cat", "dog", "fish" };
					object[] array2 = array1;
					array2[0] = 5;
					break;
				case 7: //Case7: Unable to cast object of type 'System.Text.StringBuilder' to type 'System.IO.StreamReader'. 
						// System.InvalidCastException - Handles errors generated during typecasting.
					StringBuilder reference1 = new StringBuilder();
					object reference2 = reference1;
					System.IO.StreamReader reference3 = (System.IO.StreamReader)reference2;
					break;
				case 8: //Case8: System.OutOfMemoryException - Handles errors generated from insufficient free memory. 
					string maxvalue = new string('a', int.MaxValue);
					break;
				case 9: //Case9: System.StackOverflowException - Handles errors generated from stack overflow.
						// Write call number and call this method again.
						// ... The stack will eventually overflow.
					Recursive(0);
					break;

				default: break;
			}
		}
    }
}
