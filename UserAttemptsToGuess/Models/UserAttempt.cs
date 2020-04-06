using System;
using System.ComponentModel.DataAnnotations;

namespace UserAttemptsToGuess.Models
{
    public class UserAttempt
    {
        [Key]
        public int Id { get; set; }
        public string SessionKey { get; set; }
        public int InputNumber { get; set; }
        public DateTime? AttemptDate { get; set; }
    }
}
