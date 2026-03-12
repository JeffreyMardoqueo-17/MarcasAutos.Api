param(
    [string]$filter = ""
)

$project = ".\MarcasAutos.Tests\MarcasAutos.Tests.csproj"
$settings = ".\coverage.runsettings"

if ($filter -ne "") {
    dotnet test $project --filter $filter --settings $settings --logger "console;verbosity=normal"
} else {
    dotnet test $project --filter "Category!=ShouldFail" --settings $settings --collect:"XPlat Code Coverage" --logger "console;verbosity=normal"
}
