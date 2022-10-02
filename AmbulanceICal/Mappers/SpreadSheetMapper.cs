using AmbulanceICal.Models;

namespace AmbulanceICal.Mappers
{
    public static class SpreadSheetMapper
    {
        public static List<SchemaModel> Map(GoogleSpreadSheetResponse model, string team, string vehicle)
        {
            var schemaModels = GetSchemaModels(model, team, vehicle);

            return schemaModels;
        }

        private static List<SchemaModel> GetSchemaModels(GoogleSpreadSheetResponse model, string team, string vehicle)
        {
            int vehicleRowIndex = GetVehicleRowIndex(model, vehicle);
            int rowIndex = GetTeamRowIndex(model, team, vehicleRowIndex);
            var shifts = GetShiftsByRowIndex(model, rowIndex, team);

            return GetSchemaModels(model, shifts, team, vehicle);
        }

        private static int GetVehicleRowIndex(GoogleSpreadSheetResponse model, string vehicle)
        {
            int rowIndex = 0;
            for (int i = 0; i < model.Table?.Rows?.Count; i++)
            {
                for (int j = 0; j < model.Table?.Rows?[i].C?.Count; j++)
                {
                    var value = model.Table?.Rows?[i].C?[j];
                    if (value != null && value.V != null && value.V.Contains(vehicle))
                    {
                        if (value.V.Contains(vehicle))
                        {
                            return i;
                        }
                    }
                }
            }

            return rowIndex;
        }

        private static List<SchemaModel> GetSchemaModels(GoogleSpreadSheetResponse model, Dictionary<int, string> shifts, string team, string vehicle)
        {
            var list = new List<SchemaModel>();
            foreach (var shift in shifts)
            {
                var week = GetWeek(model, shift.Key);
                var day = GetDay(model, shift.Key);
                var workHours = GetWorkHours(model, shift.Value);
                var dateModel = new SchemaModel
                {
                    Team = $"{vehicle} - {team}",
                    Day = day,
                    Week = week,
                    WorkHours = workHours,
                    Shift = shift
                };
                list.Add(dateModel);
            }

            return list;
        }

        private static string GetWorkHours(GoogleSpreadSheetResponse model, string value)
        {
            var rowCount = model.Table?.Rows?.Count;
            for (int i = 30; i < rowCount; i++)
            {
                var row = model.Table?.Rows?[i];
                if(row != null && row.C?[1] != null)
                {
                    var identifier = row?.C?[1].V;
                    if (identifier != null && identifier.Contains(value))
                    {
                        var hours = row?.C?[3].V;
                        return hours;
                    }
                }
            }
            return "";
        }

        private static int GetDay(GoogleSpreadSheetResponse model, int key)
        {
            var dayIndex = 2;
            var dayRow = model.Table?.Rows?[dayIndex];
            var rowValue = dayRow.C?[key];
            return int.Parse(rowValue.V);
        }

        private static int GetWeek(GoogleSpreadSheetResponse model, int key)
        {
            var weekIndex = 0;
            var weekRow = model.Table?.Rows?[weekIndex];
            var rowValue = weekRow.C?[key];
            int week;
            if (rowValue != null && rowValue.V != null)
            {
                int.TryParse(rowValue.V, out week);
                if (week != 0)
                    return week;

                rowValue = weekRow.C?[key + 1];
                if (rowValue != null && rowValue.V != null)
                {
                    int.TryParse(rowValue.V, out week);
                    if (week != 0)
                        return week;
                }

            }
            //Check backwards for week
            for (int i = key; i > 0; --i)
            {
                rowValue = weekRow.C?[i];
                if (rowValue != null && rowValue.V != null)
                {
                    int.TryParse(rowValue.V, out week);
                    if (week != 0)
                        return week;
                }
            }

            return 0;

        }

        private static Dictionary<int, string> GetShiftsByRowIndex(GoogleSpreadSheetResponse model, int rowIndex, string team)
        {
            var values = new Dictionary<int, string>();
            for (int i = 0; i < model.Table?.Rows?[rowIndex]?.C?.Count; i++)
            {
                var value = model.Table?.Rows?[rowIndex]?.C?[i];
                if (value != null && value.V != null)
                {
                    if (!string.IsNullOrEmpty(value.V) && !value.V.Contains(team))
                    {
                        values.Add(i, value.V);
                    }
                }
            }

            Console.WriteLine($"Found {values.Count} shifts for {team}!");
            return values;
        }

        private static int GetTeamRowIndex(GoogleSpreadSheetResponse model, string team, int vehicleRowIndex)
        {
            int rowIndex = 0;
            for (int i = vehicleRowIndex; i < model.Table?.Rows?.Count; i++)
            {
                for (int j = 0; j < model.Table?.Rows?[i].C?.Count; j++)
                {
                    var value = model.Table?.Rows?[i].C?[j];
                    if (value != null && value.V != null && value.V.Contains(team))
                    {
                        if (value.V.Contains(team))
                        {
                            return i;
                        }
                    }
                }
            }

            return rowIndex;
        }
    }
}

