param(
    [string]$filter = ""
)

$project = ".\MarcasAutos.Tests\MarcasAutos.Tests.csproj"

if ($filter -ne "") {
    dotnet test $project --filter $filter --logger "console;verbosity=normal"
} else {
    dotnet test $project --filter "Category!=ShouldFail" --logger "console;verbosity=normal"
}
