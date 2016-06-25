using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.AL;
using ICSharpCode.NRefactory.AL.Completion;
using ICSharpCode.NRefactory.TypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace ICSharpCode.CodeCompletion
{
     public class SourceCodeParsedClass
    {
        public string Name {get;set;}
        public Modifiers ItemModifiers { get; set; }
        public List<SourceCodeParsedMember> AllMembers = new List<SourceCodeParsedMember>();
        public TextLocation Location { get; set; }
        public TextLocation EndLocation { get; set; }
    }
    public class SourceCodeParsedMember
    {
        public string Name {get;set;} 
        public string ReturnType {get;set;}
        public Modifiers ItemModifiers { get; set; }
        public TextLocation Location { get; set; }
        public TextLocation EndLocation { get; set; }
        public string MemberType { get; set; }
    }
  public class SourceCodeParsingResult
    {
      public List<SourceCodeParsedClass> Classes = new List<SourceCodeParsedClass>();
      public List<SourceCodeParsedMember> Enums = new List<SourceCodeParsedMember>();
      public List<SourceCodeParsedMember> Delegates = new List<SourceCodeParsedMember>();
      public List<SourceCodeParsedClass> Interfaces = new List<SourceCodeParsedClass>();
      public List<SourceCodeParsedClass> Structs = new List<SourceCodeParsedClass>();
      public static SourceCodeParsingResult Create(string source)
      {
          try
          {
              string temp = Path.GetTempFileName();
 
              ALParser parser = new ALParser();
              
              var syntaxTree = parser.Parse(source, source);
              SourceCodeParsingResult sr = new SourceCodeParsingResult(syntaxTree);
              return sr;
          }
          catch
          {

          }
          return null;
      }
      string GetTypeS(EntityDeclaration mem)
      {

          if (mem is PropertyDeclaration)
              return "property";
          else if (mem is FieldDeclaration)
              return "field";
          else if (mem is EventDeclaration)
              return "event";
          else if (mem is OperatorDeclaration)
              return "operator";
          else if (mem is ConstructorDeclaration)
              return "method";
          else if (mem is DestructorDeclaration)
              return "method";
          else return "method";
      }
      public SourceCodeParsingResult(SyntaxTree tree)
      {
          try
          {
              Success = (tree.Errors.Count == 0);
              var testClass = tree.Descendants.OfType<TypeDeclaration>();
              StringBuilder sb = new StringBuilder();
              foreach (TypeDeclaration decl in testClass)
              {
                  if (decl.ClassType == ClassType.Class)
                  {
                      SourceCodeParsedClass cla = new SourceCodeParsedClass();
                      cla.Name = decl.Name;
                      cla.ItemModifiers = decl.Modifiers;
                      cla.Location = decl.StartLocation;
                      cla.EndLocation = decl.EndLocation;
                      var testClassAttributes = decl.Members;
                      foreach (EntityDeclaration mem in testClassAttributes)
                      {
                          SourceCodeParsedMember member = new SourceCodeParsedMember();
                          member.ItemModifiers = mem.Modifiers;
                          member.ReturnType = mem.ReturnType.ToString();
                          member.Location = mem.StartLocation;
                          member.EndLocation = mem.EndLocation;
                          if (mem.Name != "")
                              member.Name = mem.Name;
                          else
                              member.Name = mem.ToString();
                          member.MemberType = GetTypeS(mem);
                     
                          cla.AllMembers.Add(member);

                      }
                      Classes.Add(cla);
                  }
                  else if (decl.ClassType == ClassType.Struct)
                  {
                      SourceCodeParsedClass cla = new SourceCodeParsedClass();
                      cla.Name = decl.Name;
                      cla.ItemModifiers = decl.Modifiers;
                      cla.Location = decl.StartLocation;
                      cla.EndLocation = decl.EndLocation;
                      var testClassAttributes = decl.Members;
                      foreach (EntityDeclaration mem in testClassAttributes)
                      {
                          SourceCodeParsedMember member = new SourceCodeParsedMember();
                          member.ItemModifiers = mem.Modifiers;
                          member.ReturnType = mem.ReturnType.ToString();
                          member.Name = mem.Name;
                          member.Location = mem.StartLocation;
                          member.EndLocation = mem.EndLocation;
                          member.MemberType = GetTypeS(mem);

                          cla.AllMembers.Add(member);

                      }
                      Structs.Add(cla);
                  }
                  else if (decl.ClassType == ClassType.Interface)
                  {
                      SourceCodeParsedClass cla = new SourceCodeParsedClass();
                      cla.Name = decl.Name;
                      cla.ItemModifiers = decl.Modifiers;
                      cla.Location = decl.StartLocation;
                      cla.EndLocation = decl.EndLocation;
                      var testClassAttributes = decl.Members;
                      foreach (EntityDeclaration mem in testClassAttributes)
                      {

                          SourceCodeParsedMember member = new SourceCodeParsedMember();
                          member.ItemModifiers = mem.Modifiers;
                          member.ReturnType = mem.ReturnType.ToString();
                          member.Name = mem.Name;
                          member.Location = mem.StartLocation;
                          member.EndLocation = mem.EndLocation;
                          member.MemberType = GetTypeS(mem);
                          cla.AllMembers.Add(member);
                      }

                      Interfaces.Add(cla);
                  }
                  else if (decl.ClassType == ClassType.Enum)
                  {
                      SourceCodeParsedMember cla = new SourceCodeParsedMember();
                      cla.Name = decl.Name;
                      cla.ItemModifiers = decl.Modifiers;
                      cla.Location = decl.StartLocation;
                      cla.EndLocation = decl.EndLocation;
                      Enums.Add(cla);
                  }
                  else
                  {
                      SourceCodeParsedMember cla = new SourceCodeParsedMember();
                      cla.Name = decl.Name;
                      cla.ItemModifiers = decl.Modifiers;
                      cla.Location = decl.StartLocation;
                      cla.EndLocation = decl.EndLocation;
                      Delegates.Add(cla);
                  }
              }
          }
          catch
          {
              Success = false;
          }
      }
  
      public bool Success { get; set; }

      static int GetModifierOffset(Modifiers modifier)
      {
          if ((modifier & Modifiers.Public) == Modifiers.Public)
          {
              return 0;
          }
          if ((modifier & Modifiers.Protected) == Modifiers.Protected)
          {
              return 3;
          }
          if ((modifier & Modifiers.Internal) == Modifiers.Internal)
          {
              return 4;
          }
          return 2;
      }
      public static AutoListIcons GetIcon(string type, Modifiers mod)
      {
          AutoListIcons imageIndex = AutoListIcons.iClass;
          if (type == "field")
              return (AutoListIcons)((int)AutoListIcons.iField + GetModifierOffset(mod));
          else if (type == "method")
              return (AutoListIcons)((int)AutoListIcons.iMethod + GetModifierOffset(mod));
          else if (type == "class")
              return (AutoListIcons)((int)AutoListIcons.iClass + GetModifierOffset(mod));
          else if (type == "struct")
              return (AutoListIcons)((int)AutoListIcons.iStructure + GetModifierOffset(mod));
          else if (type == "enum")
              return (AutoListIcons)((int)AutoListIcons.iEnum + GetModifierOffset(mod));
          else if (type == "delegate")
              return (AutoListIcons)((int)AutoListIcons.iDelegate + GetModifierOffset(mod));
          else if (type == "interface")
              return (AutoListIcons)((int)AutoListIcons.iInterface + GetModifierOffset(mod));
          else if (type == "property")
              return (AutoListIcons)((int)AutoListIcons.iProperties + GetModifierOffset(mod));
          else if (type == "event")
              return (AutoListIcons)((int)AutoListIcons.iEvent + GetModifierOffset(mod));
          else if (type == "operator")
              return (AutoListIcons)((int)AutoListIcons.iOperator + GetModifierOffset(mod));
          else return AutoListIcons.iValueType;

      }


    }

  public enum AutoListIcons
  {
      //Class Icons
      iClass = 0,
      iClassFriend,
      iClassPrivate,
      iClassProtected,
      iClassSealed,
      iClassShortCut,

      //Constant Icons
      iConstant,
      iConstantFriend,
      iConstantPrivate,
      iConstantProtected,
      iConstantSealed,
      iConstantShortCut,

      //Delegate Icons
      iDelegate,
      iDelegateFriend,
      iDelegatePrivate,
      iDelegateProtected,
      iDelegateSealed,
      iDelegateShortCut,

      //Enum Icons
      iEnum,
      iEnumFriend,
      iEnumPrivate,
      iEnumProtected,
      iEnumSealed,
      iEnumShortCut,

      //EnumItem Icons
      iEnumItem,
      iEnumItemFriend,
      iEnumItemPrivate,
      iEnumItemProtected,
      iEnumItemSealed,
      iEnumItemShortCut,

      //Event Icons
      iEvent,
      iEventFriend,
      iEventPrivate,
      iEventProtected,
      iEventSealed,
      iEventShortCut,

      //Exception Icons
      iException,
      iExceptionFriend,
      iExceptionPrivate,
      iExceptionProtected,
      iExceptionSealed,
      iExceptionShortCut,

      //Field Icons
      iField,
      iFieldFriend,
      iFieldPrivate,
      iFieldProtected,
      iFieldSealed,
      iFieldShortCut,

      //Interface Icons
      iInterface,
      iInterfaceFriend,
      iInterfacePrivate,
      iInterfaceProtected,
      iInterfaceSealed,
      iInterfaceShortCut,

      //Macro Icons
      iMacro,
      iMacroFriend,
      iMacroPrivate,
      iMacroProtected,
      iMacroSealed,
      iMacroShortCut,

      //Map Icons
      iMap,
      iMapFriend,
      iMapPrivate,
      iMapProtected,
      iMapSealed,
      iMapShortCut,

      //MapItem Icons
      iMapItem,
      iMapItemFriend,
      iMapItemPrivate,
      iMapItemProtected,
      iMapItemSealed,
      iMapItemShortCut,

      //Method Icons
      iMethod,
      iMethodFriend,
      iMethodPrivate,
      iMethodProtected,
      iMethodSealed,
      iMethodShortCut,

      //MethodOverloaded Icons
      iMethodOverloaded,
      iMethodOverloadedFriend,
      iMethodOverloadedPrivate,
      iMethodOverloadedProtected,
      iMethodOverloadedSealed,
      iMethodOverloadedShortCut,

      //Module Icons
      iModule,
      iModuleFriend,
      iModulePrivate,
      iModuleProtected,
      iModuleSealed,
      iModuleShortCut,

      //NameSpace Icons
      iNamespace,
      iNamespaceFriend,
      iNamespacePrivate,
      iNamespaceProtected,
      iNamespaceSealed,
      iNamespaceShortCut,


      //Object Icons
      iObject,
      iObjectFriend,
      iObjectPrivate,
      iObjectProtected,
      iObjectSealed,
      iObjectShortCut,

      //Operator Icons
      iOperator,
      iOperatorFriend,
      iOperatorPrivate,
      iOperatorProtected,
      iOperatorSealed,
      iOperatorShortCut,

      //Properties Icons
      iProperties,
      iPropertiesFriend,
      iPropertiesPrivate,
      iPropertiesProtected,
      iPropertiesSealed,
      iPropertiesShortCut,

      //Structure Icons
      iStructure,
      iStructureFriend,
      iStructurePrivate,
      iStructureProtected,
      iStructureSealed,
      iStructureShortCut,

      //Template Icons
      iTemplate,
      iTemplateFriend,
      iTemplatePrivate,
      iTemplateProtected,
      iTemplateSealed,
      iTemplateShortCut,

      //Type Icons
      iType,
      iTypeFriend,
      iTypePrivate,
      iTypeProtected,
      iTypeSealed,
      iTypeShortCut,

      //TypeDef Icons
      iTypeDef,
      iTypeDefFriend,
      iTypeDefPrivate,
      iTypeDefProtected,
      iTypeDefSealed,
      iTypeDefShortCut,

      //Union Icons
      iUnion,
      iUnionFriend,
      iUnionPrivate,
      iUnionProtected,
      iUnionSealed,
      iUnionShortCut,

      // ValueType Icons
      iValueType,
      iValueTypeFriend,
      iValueTypePrivate,
      iValueTypeProtected,
      iValueTypeSealed,
      iValueTypeShortCut
  }
}
