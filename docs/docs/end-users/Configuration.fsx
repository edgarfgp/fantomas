(**
---
category: End-users
categoryindex: 1
index: 3
---
*)

(**
# Configuration
Fantomas ships with a limited series of options.
These can be stored in an [.editorconfig](https://editorconfig.org/) file and will be picked up automatically by the
commandline.  
Your IDE should respect your settings, however the implementation of that is editor specific. Setting the configuration via
UI might be available depending on the IDE.
*)

#r "../../../src/Fantomas/bin/Release/net6.0/Fantomas.FCS.dll"
#r "../../../src/Fantomas/bin/Release/net6.0/Fantomas.Core.dll"

printf $"version: {Fantomas.Core.CodeFormatter.GetVersion()}"
(*** include-output  ***)

(**
## Usage
Inside .editorconfig you can specify the file extension and code location to be use per config:
```
[*.fs]
fsharp_space_before_uppercase_invocation = true

#\ Write a comment by starting the line with a '#'
[*.{fs,fsx,fsi}]
fsharp_bar_before_discriminated_union_declaration = true

#\ Apply specific settings for a targeted subfolder
[src/Elmish/View.fs]
fsharp_multiline_bracket_style = stroustrup
```
*)

(**
## Trying your settings via the online tool
You can quickly try your settings via the <a href="https://fsprojects.github.io/fantomas-tools/#/fantomas/preview" target="_blank">online tool</a>.

<img src="{{root}}/online_tool_usage.gif" alt="drawing" width="100%"/>
*)

open Fantomas.Core

let formatCode input configIndent =
    async {
        let! result = CodeFormatter.FormatDocumentAsync(false, input, configIndent)
        printf $"%s{result.Code}"
    }
    |> Async.RunSynchronously

(**
## Settings recommendations
Fantomas ships with a series of settings that you can use freely depending  on your case.  
However, there are settings that we do not recommend and generally should not be used.
<p><fantomas-setting-icon green></fantomas-setting-icon><strong>Safe to change:</strong> Settings that aren't attached to any guidelines. Depending on your team or your own preferences, feel free to change these as it's been agreed on the codebase, however, you can always use it's defaults.</p>
<p><fantomas-setting-icon orange></fantomas-setting-icon><strong>Use with caution:</strong> Settings where it is not recommended to change the default value. They might lead to incomplete results.</p>
<p><fantomas-setting-icon red></fantomas-setting-icon><strong>Do not use:</strong> Settings that don't follow any guidelines.</p>
<p><fantomas-setting-icon gr></fantomas-setting-icon><strong>G-Research:</strong> G-Research styling guide. If you use one of these, for consistency reasons you should use all of them.</p>
<p><i class="bi bi-clipboard"></i> <strong>Copy button:</strong> This copies the `.editorconfig` setting text you need to change the default. ⚠️ The copied text will not contain the default value.
*)

(**
## Auxiliary settings

<fantomas-setting name="indent_size" orange clip="2"></fantomas-setting>

`indent_size` has to be between 1 and 10.

This preference sets the indentation
The common values are 2 and 4.  
The same indentation is ensured to be consistent in a source file.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.IndentSize}"
(*** include-output ***)

formatCode
    """ 
    let inline selectRandom (f: _ []) =
      let r = random 1.0

      let rec find =
          function
          | 0 -> fst f.[0]
          | n when r < snd f.[n] -> fst f.[n]
          | n -> find (n - 1)

      find <| f.Length - 1
    """
    { FormatConfig.Default with
        IndentSize = 2 }
(*** include-output ***)
(**
<fantomas-setting name="max_line_length" green clip="100"></fantomas-setting>

`max_line_length` has to be an integer greater or equal to 60.  
This preference sets the column where we break F# constructs into new lines.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxLineLength}"
(*** include-output ***)

formatCode
    """ 
    match myValue with
    | Some foo -> someLongFunctionNameThatWillTakeFooAndReturnsUnit foo
    | None -> printfn "nothing"
    """
    { FormatConfig.Default with
        MaxLineLength = 60 }
(*** include-output ***)

(**
<fantomas-setting name="end_of_line" green clip="lf"></fantomas-setting>

`end_of_line` determines the newline character, `lf` will add `\n` where `crlf` will add `\r\n`.  
`cr` is not supported by the F# language spec.  
If not set by the user, the default value is determined by `System.Environment.NewLine`.
*)

(**
<fantomas-setting name="insert_final_newline" orange clip="false"></fantomas-setting>

Adds a final newline character at the end of the file.  
<a href="https://stackoverflow.com/questions/729692/why-should-text-files-end-with-a-newline" target="_blank">Why should text files end with a newline?</a>
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.InsertFinalNewline.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
    let a = 42
    """
    { FormatConfig.Default with
        InsertFinalNewline = false }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_space_before_parameter" orange clip="false"></fantomas-setting>

Add a space after the name of a function and before the opening parenthesis of the first parameter.  
This setting influences function definitions.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.SpaceBeforeParameter.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
   let value (a: int) = x
   let DumpTrace() = ()
    """
    { FormatConfig.Default with
        SpaceBeforeParameter = false }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_space_before_lowercase_invocation" orange clip="false"></fantomas-setting>

Add a space after the name of a lowercased function and before the opening parenthesis of the first argument.  
This setting influences function invocation in expressions and patterns.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.SpaceBeforeLowercaseInvocation.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
value (a, b)
startTimer ()

match x with
| value (a, b) -> ()
    """
    { FormatConfig.Default with
        SpaceBeforeLowercaseInvocation = false }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_space_before_uppercase_invocation" green gr clip="true"></fantomas-setting>

Add a space after the name of a uppercase function and before the opening parenthesis of the first argument.  
This setting influences function invocation in expressions and patterns.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.SpaceBeforeUppercaseInvocation.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
Value(a, b)
person.ToString()

match x with
| Value(a, b) -> ()
    """
    { FormatConfig.Default with
        SpaceBeforeUppercaseInvocation = true }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_space_before_class_constructor" orange clip="true"></fantomas-setting>

Add a space after a type name and before the class constructor.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.SpaceBeforeClassConstructor.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
    type Person() =
        class
        end
    """
    { FormatConfig.Default with
        SpaceBeforeClassConstructor = true }

(*** include-output ***)

(**
<fantomas-setting name="fsharp_space_before_member" green gr clip="true"></fantomas-setting>

Add a space after a member name and before the opening parenthesis of the first parameter.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.SpaceBeforeMember.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
    type Person() =
        member this.Walk(distance: int) = ()
        member this.Sleep() = ignore
        member __.singAlong() = ()
        member __.swim(duration: TimeSpan) = ()
    """
    { FormatConfig.Default with
        SpaceBeforeMember = true }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_space_before_colon" green clip="true"></fantomas-setting>

Add a space before `:`. Please note that not every `:` is controlled by this setting.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.SpaceBeforeColon.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
   type Point = { x: int; y: int }
   let myValue: int = 42
   let update (msg: Msg) (model: Model) : Model = model
    """
    { FormatConfig.Default with
        SpaceBeforeColon = true }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_space_after_comma" orange clip="false"></fantomas-setting>

Adds a space after `,` in tuples.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.SpaceAfterComma.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
    myValue.SomeFunction(foo, bar, somethingElse)
    (a, b, c)
    """
    { FormatConfig.Default with
        SpaceAfterComma = false }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_space_before_semicolon" green gr clip="true"></fantomas-setting>

Adds a space before `;` in records, arrays, lists, etc.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.SpaceBeforeSemicolon.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
    let a = [ 1 ; 2 ; 3 ]
    let b = [| foo ; bar |]
    type C = { X: int ; Y: int }
    """
    { FormatConfig.Default with
        SpaceBeforeSemicolon = true }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_space_after_semicolon" orange clip="false"></fantomas-setting>

Adds a space after `;` in records, arrays, lists, etc.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.SpaceAfterSemicolon.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
    let a = [ 1; 2; 3 ]
    let b = [| foo; bar |]
    type C = { X: int; Y: int }
    """
    { FormatConfig.Default with
        SpaceAfterSemicolon = false }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_space_around_delimiter" orange clip="false"></fantomas-setting>

Adds a space around delimiters like `[`,`[|`,{`.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.SpaceAroundDelimiter.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
    let a = [ 1;2;3 ]
    let b = [| 4;5;6 |]
    """
    { FormatConfig.Default with
        SpaceAroundDelimiter = false }
(*** include-output ***)

(**
## Maximum width constraints

Settings that control the max width of certain expressions.

<fantomas-setting name="fsharp_max_if_then_short_width" orange></fantomas-setting>

Control the maximum length for which if/then expression without an else expression can be on one line.  
The [Microsoft F# style guide](https://docs.microsoft.com/en-us/dotnet/fsharp/style-guide/formatting#formatting-if-expressions) recommends to never write such an expression in one line.

> If the else expression is absent, it is recommended to never to write the entire expression in one line.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxIfThenShortWidth}"
(*** include-output ***)

formatCode
    """ 
    if a then 
        ()
    """
    { FormatConfig.Default with
        MaxIfThenShortWidth = 15 }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_max_if_then_else_short_width" green clip="80"></fantomas-setting>

Fantomas by default follows the if/then/else conventions listed in the [Microsoft F# style guide](https://docs.microsoft.com/en-us/dotnet/fsharp/style-guide/formatting#formatting-if-expressions).  
This setting facilitates this by determining the maximum character width where the if/then/else expression stays in one line.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxIfThenElseShortWidth}"
(*** include-output ***)

formatCode
    """ 
    if myCheck then truth else bogus
    """
    { FormatConfig.Default with
        MaxIfThenElseShortWidth = 10 }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_max_infix_operator_expression" green clip="100"></fantomas-setting>

Control the maximum length for which infix expression can be on one line.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxInfixOperatorExpression}"
(*** include-output ***)

formatCode
    """ 
    let WebApp =
        route "/ping" >=> authorized >=> text "pong"
    """
    { FormatConfig.Default with
        MaxInfixOperatorExpression = 20 }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_max_record_width" green clip="60"></fantomas-setting>

Control the maximum width for which records should be in one line.

Requires `fsharp_record_multiline_formatter` to be `character_width` to take effect.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxRecordWidth}"
(*** include-output ***)

formatCode
    """ 
    type MyRecord = { X: int; Y: int; Length: int }
    let myInstance = { X = 10; Y = 20; Length = 90 }
    """
    { FormatConfig.Default with
        MaxRecordWidth = 20 }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_max_record_number_of_items" green clip="2"></fantomas-setting>

Control the maximum number of fields for which records should be in one line.

Requires `fsharp_record_multiline_formatter` to be
`number_of_items` to take effect.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxRecordNumberOfItems}"
(*** include-output ***)

formatCode
    """ 
    type R = { x: int }

    type S = { x: int; y: string }

    type T = { x: int; y: string; z: float }

    let myRecord = { r = 3 }

    let myRecord' = { r with x = 3 }

    let myRecord'' = { r with x = 3; y = "hello" }

    let myRecord''' = { r with x = 3; y = "hello"; z = 0.0 }
    """
    { FormatConfig.Default with
        MaxRecordNumberOfItems = 2
        RecordMultilineFormatter = MultilineFormatterType.NumberOfItems }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_record_multiline_formatter" green clip="number_of_items"></fantomas-setting>

Split records expressions/statements into multiple lines based on the given condition.  
`character_width` uses character count of the expression, controlled by `fsharp_max_record_width`.  
`number_of_items` uses the number of fields in the record, controlled by `fsharp_max_record_number_of_items`.

Note that in either case, record expressions/statements are still governed by `max_line_length`.
*)

(*** hide ***)
printfn $"Default = {MultilineFormatterType.ToConfigString FormatConfig.Default.RecordMultilineFormatter}"
(*** include-output ***)

formatCode
    """ 
    type R = { x: int }

    type S = { x: int; y: string }

    let myRecord = { r = 3 }

    let myRecord' = { r with x = 3 }

    let myRecord'' = { r with x = 3; y = "hello" }
    """
    { FormatConfig.Default with
        RecordMultilineFormatter = MultilineFormatterType.NumberOfItems }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_max_array_or_list_width" green clip="100"></fantomas-setting>

Control the maximum width for which lists and arrays can be in one line. 

Requires `fsharp_array_or_list_multiline_formatter` to be `character_width` to take effect
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxArrayOrListWidth}"
(*** include-output ***)

formatCode
    """ 
    let myArray = [| one; two; three |]
    """
    { FormatConfig.Default with
        MaxArrayOrListWidth = 20 }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_max_array_or_list_number_of_items" green clip="2"></fantomas-setting>

Control the maximum number of elements for which lists and arrays can be in one line.

Requires `fsharp_array_or_list_multiline_formatter` to be `number_of_items` to take effect.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxArrayOrListNumberOfItems}"
(*** include-output ***)

formatCode
    """ 
    let myList = [ one; two ]
    let myArray = [| one; two; three |]
    """
    { FormatConfig.Default with
        MaxArrayOrListNumberOfItems = 2
        ArrayOrListMultilineFormatter = MultilineFormatterType.NumberOfItems }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_array_or_list_multiline_formatter" green clip="number_of_items"></fantomas-setting>

Split arrays and lists into multiple lines based on the given condition.  
`character_width` uses character count of the expression, controlled by `fsharp_max_array_or_list_width`.  
`number_of_items` uses the number of elements in the array or list, controlled by `fsharp_max_array_or_list_number_of_items`.

Note that in either case, list expressions are still governed by `max_line_length`.
*)

(*** hide ***)
printfn $"Default = {MultilineFormatterType.ToConfigString FormatConfig.Default.ArrayOrListMultilineFormatter}"
(*** include-output ***)

formatCode
    """ 
    let myArray = [| one; two; three |]
    """
    { FormatConfig.Default with
        ArrayOrListMultilineFormatter = MultilineFormatterType.NumberOfItems }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_max_value_binding_width" green clip="100"></fantomas-setting>

Control the maximum expression width for which let and member value/property bindings should be in one line.  
The width is that of the pattern for the binding plus the right-hand expression but not the keywords (e.g. "let").
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxValueBindingWidth}"
(*** include-output ***)

formatCode
    """ 
    let title = "Great title of project"
    type MyType() =
        member this.HelpText = "Some help text"
    """
    { FormatConfig.Default with
        MaxValueBindingWidth = 10 }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_max_function_binding_width" green clip="40"></fantomas-setting>

Control the maximum width for which function and member bindings should be in one line.  
In contrast to `fsharp_max_value_binding_width`, only the right-hand side expression of the binding is measured.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxFunctionBindingWidth}"
(*** include-output ***)

formatCode
    """ 
    let title = "Great title of project"
    type MyType() =
        member this.HelpText = "Some help text"
    """
    { FormatConfig.Default with
        MaxFunctionBindingWidth = 10 }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_max_dot_get_expression_width" green clip="100"></fantomas-setting>

Control the maximum width for which (nested) [SynExpr.DotGet](https://fsharp.github.io/fsharp-compiler-docs/reference/fsharp-compiler-syntax-synexpr.html#DotGet) expressions should be in one line.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MaxDotGetExpressionWidth}"
(*** include-output ***)

formatCode
    """ 
   let job = JobBuilder.UsingJobData(jobDataMap).Create<WrapperJob>().Build()
    """
    { FormatConfig.Default with
        MaxDotGetExpressionWidth = 60 }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_multiline_bracket_style" green gr clip="stroustrup"></fantomas-setting>

`Cramped` The default way in F# to format brackets.  
`Aligned` Alternative way of formatting records, arrays and lists. This will align the braces at the same column level.  
`Stroustrup` Allow for easier reordering of members and keeping the code succinct. 
*)

(*** hide ***)
printfn $"Default = {MultilineBracketStyle.ToConfigString FormatConfig.Default.MultilineBracketStyle}"
(*** include-output ***)

formatCode
    """ 
    let myRecord =
        { Level = 1
          Progress = "foo"
          Bar = "bar"
          Street = "Bakerstreet"
          Number = 42 }

    type Range =
        { From: float
          To: float
          FileName: string }

    let a =
        [| (1, 2, 3)
           (4, 5, 6)
           (7, 8, 9)
           (10, 11, 12)
           (13, 14, 15)
           (16, 17,18)
           (19, 20, 21) |]
    """
    { FormatConfig.Default with
        MultilineBracketStyle = Aligned }
(*** include-output ***)

formatCode
    """ 
    let myRecord =
        { Level = 1
          Progress = "foo"
          Bar = "bar"
          Street = "Bakerstreet"
          Number = 42 }

    type Range =
        { From: float
          To: float
          FileName: string }

    let a =
        [| (1, 2, 3)
           (4, 5, 6)
           (7, 8, 9)
           (10, 11, 12)
           (13, 14, 15)
           (16, 17,18)
           (19, 20, 21) |]
    """
    { FormatConfig.Default with
        MultilineBracketStyle = Stroustrup }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_newline_before_multiline_computation_expression" green clip="false"></fantomas-setting>

Insert a newline before a computation expression that spans multiple lines
*)

(*** hide ***)
printfn
    $"Default = {FormatConfig.Default.NewlineBeforeMultilineComputationExpression
                     .ToString()
                     .ToLower()}"
(*** include-output ***)

formatCode
    """ 
    let something =
        task {
            let! thing = otherThing ()
            return 5
        }
    """
    { FormatConfig.Default with
        NewlineBeforeMultilineComputationExpression = false }
(*** include-output ***)

(**
## G-Research style

A series of settings required to conform with the [G-Research style guide](https://github.com/G-Research/fsharp-formatting-conventions).  
From a consistency point of view, it is recommend to enable all these settings instead of cherry-picking a few.

<fantomas-setting name="fsharp_newline_between_type_definition_and_members" green gr clip="false"></fantomas-setting>

Adds a new line between a type definition and its first member.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.NewlineBetweenTypeDefinitionAndMembers.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
type Range =
    { From: float
      To: float }

    member this.Length = this.To - this.From
    """
    { FormatConfig.Default with
        NewlineBetweenTypeDefinitionAndMembers = false }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_align_function_signature_to_indentation" green gr clip="true"></fantomas-setting>

When a function signature exceeds the `max_line_length`, Fantomas will put all parameters on separate lines.  
This setting also places the equals sign and return type on a new line.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.AlignFunctionSignatureToIndentation.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
[<FunctionName("FormatCode")>]
let run 
    ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "{*any}")>] req: HttpRequest)
    (log: ILogger)
    : HttpResponse =
    Http.main CodeFormatter.GetVersion format FormatConfig.FormatConfig.Default log req
    """
    { FormatConfig.Default with
        AlignFunctionSignatureToIndentation = true }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_alternative_long_member_definitions" green gr clip="true"></fantomas-setting>

Provides an alternative way of formatting long member and constructor definitions,
where the difference is mainly in the equal sign and returned type placement.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.AlternativeLongMemberDefinitions.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
type C
    (
        aVeryLongType: AVeryLongTypeThatYouNeedToUse,
        aSecondVeryLongType: AVeryLongTypeThatYouNeedToUse,
        aThirdVeryLongType: AVeryLongTypeThatYouNeedToUse
    ) =
    class
    end

type D() =
    member _.LongMethodWithLotsOfParameters
        (
            aVeryLongParam: AVeryLongTypeThatYouNeedToUse,
            aSecondVeryLongParam: AVeryLongTypeThatYouNeedToUse,
            aThirdVeryLongParam: AVeryLongTypeThatYouNeedToUse
        ) : ReturnType =
        42
    """
    { FormatConfig.Default with
        AlternativeLongMemberDefinitions = true }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_multi_line_lambda_closing_newline" green gr clip="true"></fantomas-setting>

Places the closing parenthesis of a multiline lambda argument on the next line.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.MultiLineLambdaClosingNewline.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
let printListWithOffset a list1 =
    List.iter
        (fun { ItemOne = a } ->
            // print
            printfn "%s" a)
        list1

let printListWithOffset a list1 =
    list1
    |> List.iter
        (fun elem ->
            // print stuff
            printfn "%d" (a + elem))
    """
    { FormatConfig.Default with
        MultiLineLambdaClosingNewline = true }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_experimental_keep_indent_in_branch" orange gr clip="true"></fantomas-setting>

Breaks the normal indentation flow for the last branch of a pattern match or if/then/else expression.  
Only when the last pattern match or else branch was already at the same level of the entire match or if expression.

*This feature is considered experimental and is subject to change.*
*)

formatCode
    """ 
let main argv =
    let args = parse argv

    let instructions = Library.foo args

    if args.DryRun = RunMode.Dry then
        printfn "Would execute actions, but --dry-run was supplied: %+A" instructions
        0
    else

    // proceed with main method
    let output = Library.execute instructions
    // do more stuff
    0
    """
    { FormatConfig.Default with
        ExperimentalKeepIndentInBranch = true }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_bar_before_discriminated_union_declaration" green gr clip="true"></fantomas-setting>

Always use a `|` before every case in the declaration of a discriminated union.  
If `false`, a `|` character is used only in multiple-case discriminated unions, and is omitted in short single-case DUs.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.BarBeforeDiscriminatedUnionDeclaration.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
    type MyDU = Short of int
    """
    { FormatConfig.Default with
        BarBeforeDiscriminatedUnionDeclaration = true }

(*** include-output ***)

(**
## Other

Some additional settings that don't fit into any style guide.

<fantomas-setting name="fsharp_blank_lines_around_nested_multiline_expressions" green clip="false"></fantomas-setting>

Surround **nested** multi-line expressions with blank lines.  
Existing blank lines are always preserved (via trivia), with exception when [fsharp_keep_max_number_of_blank_lines](#fsharp_keep_max_number_of_blank_lines) is used.  
Top level expressions will always follow the [2020 blank lines revision](https://github.com/fsprojects/fantomas/blob/main/docs-old/FormattingConventions.md#2020-revision) principle.
*)

(*** hide ***)
printfn
    $"Default = {FormatConfig.Default.BlankLinesAroundNestedMultilineExpressions
                     .ToString()
                     .ToLower()}"
(*** include-output ***)

formatCode
    """ 
    let topLevelFunction () =
        printfn "Something to print"

        try
                nothing ()
        with
        | ex ->
            splash ()
        ()

    let secondTopLevelFunction () =
        // ...
        ()
    """
    { FormatConfig.Default with
        BlankLinesAroundNestedMultilineExpressions = false }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_keep_max_number_of_blank_lines" green clip="1"></fantomas-setting>

Set maximal number of consecutive blank lines to keep from original source. It doesn't change number of new blank lines generated by Fantomas.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.KeepMaxNumberOfBlankLines}"
(*** include-output ***)

formatCode
    """ 
    open Foo

    let x = 42
    """
    { FormatConfig.Default with
        KeepMaxNumberOfBlankLines = 1 }
(*** include-output ***)

(**
<fantomas-setting name="fsharp_experimental_elmish" orange clip="true"></fantomas-setting>

Applies the Stroustrup style to the final (two) array or list argument(s) in a function application.  
Note that this behaviour is also active when `fsharp_multiline_bracket_style = stroustrup`.
*)

(*** hide ***)
printfn $"Default = {FormatConfig.Default.ExperimentalElmish.ToString().ToLower()}"
(*** include-output ***)

formatCode
    """ 
let dualList =
    div
        []
        [
            h1 [] [ str "Some title" ]
            ul
                []
                [
                    for p in model.Points do
                        li [] [ str $"%i{p.X}, %i{p.Y}" ]
                ]
            hr []
        ]

let singleList =
    Html.div
        [
            Html.h1 [ str "Some title" ]
            Html.ul
                [
                    for p in model.Points do
                        Html.li [ str $"%i{p.X}, %i{p.Y}" ]
                ]
        ]
    """
    { FormatConfig.Default with
        ExperimentalElmish = true }
(*** include-output ***)

(**
<fantomas-nav previous="{{fsdocs-previous-page-link}}" next="{{fsdocs-next-page-link}}"></fantomas-nav>

*)
