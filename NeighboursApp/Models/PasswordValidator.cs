namespace neighbours.Models;
using System.ComponentModel.DataAnnotations;
using System;     
public class PasswordValidator    
{    
  public static bool ValidatePassword(string passWord)    
  {    
    int validConditions = 0;     
    foreach(char c in passWord)    
    {    
        if (c >= 'a' && c <= 'z')    
        {    
          validConditions++;    
          break;    
        }     
    }     
    if (validConditions == 0) return false;     
    foreach(char c in passWord)    
    {    
      if (c >= 'A' && c <= 'Z')    
      {    
        validConditions++;    
        break;    
      }     
    }         

    if (validConditions == 1) return false;     
    foreach(char c in passWord)    
    {    
      if (c >= '0' && c <= '9')    
      {    
        validConditions++;    
        break;    
      }     
    }     
    if (validConditions == 2) return false;     
    if(validConditions == 3)    
    {    
        char[] special = {'§', '±', '!', '@', '€', '£', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '[', ']', '{', '}', ';', ':', '/', '|', ',', '.', '<', '>', '?', '`', '~', '"', '\\', '\''}; 
    if (passWord.IndexOfAny(special) == -1) return false; 
    }
    if (passWord.Length <= 7) return false;
    else 
    return true;     
  }    
}




