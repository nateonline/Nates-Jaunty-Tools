using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NatesJauntyTools;

namespace NatesJauntyTools.Firestore
{
	[JsonConverter(typeof(Converter<Document>))]
	public class Document
	{
		public string Path { get; private set; }
		public string ID => Path.SubstringAfterLast("/");
		public DateTime CreatedTimestamp { get; private set; }
		public DateTime UpdatedTimestamp { get; private set; }

		public void InitializeFromJson(JToken id, JToken created, JToken updated)
		{
			Path = $"/{id.ToString()}";
			CreatedTimestamp = DateTime.Parse(created.ToString());
			UpdatedTimestamp = DateTime.Parse(updated.ToString());
		}


		public class Converter<T> : JsonConverter where T : Document, new()
		{
			public override bool CanConvert(Type objectType) => typeof(T).IsAssignableFrom(objectType);

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				JObject json = JObject.Load(reader);
				T newDoc = new T();
				newDoc.InitializeFromJson(json["name"], json["createTime"], json["updateTime"]);

				JObject jsonFields = (JObject)json["fields"];

				foreach (var newDocField in typeof(T).GetFields())
				{
					switch (newDocField.Name)
					{
						case nameof(newDoc.Path):
						case nameof(newDoc.ID):
						case nameof(newDoc.CreatedTimestamp):
						case nameof(newDoc.UpdatedTimestamp):
							// Ignore these fields
							break;

						default: // All other fields
							object newValue = GetValue((JObject)jsonFields[newDocField.Name]);
							Debug.Log($"Setting {typeof(T)}.{newDocField.Name} to {newValue}");
							newDocField.SetValue(newDoc, newValue);
							break;
					}


					object GetValue(JObject fieldObject)
					{
						object value = null;

						if (fieldObject["stringValue"] != null) { value = fieldObject["stringValue"].ToObject(typeof(string)); }
						if (fieldObject["booleanValue"] != null) { value = fieldObject["booleanValue"].ToObject(typeof(bool)); }
						if (fieldObject["integerValue"] != null) { value = fieldObject["integerValue"].ToObject(typeof(int)); }
						if (fieldObject["doubleValue"] != null) { value = fieldObject["doubleValue"].ToObject(typeof(double)); }
						if (fieldObject["timestampValue"] != null) { value = fieldObject["timestampValue"].ToObject(typeof(DateTime)); }

						return value;

						// Debug.Log($"Looking at json field {fieldObject}");
						// Debug.Log($"Looking at json field {fieldObject["stringValue"]}");
						// Debug.Log($"Detected json field as a {fieldType}");

						// return jsonFields[newDocField.Name].ToObject(fieldType);
					}
				}

				return newDoc;
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{


				throw new NotImplementedException();
			}
		}


		/**
		public class BuildSerializer : JsonConverter
		{
			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				JObject json = JObject.Load(reader);
				Build newBuild = new Build();

				Build.SpecFields.ForEach((Action<FieldInfo>)(sf =>
				{
					JToken jsonValue = json[sf.Name];
					Spec<object> newSpec = (Spec<object>)sf.GetValue(newBuild);
					if (jsonValue == null) { sf.SetValue(newBuild, jsonValue); }
					else
					{
						switch (newSpec.displayName)
						{
							case DisplayNames.SKU:
							case DisplayNames.TITLE:
								newSpec.Value = jsonValue.ToString();
								break;

							case DisplayNames.QUANTITY:
								newSpec.Value = jsonValue.ToObject<int?>();
								break;

							case DisplayNames.CATEGORY:
								newSpec.Value = jsonValue.ToObject<Category>();
								break;

							case DisplayNames.PRODUCT_TYPES:
								newSpec.Value = JsonConvert.DeserializeObject<List<ProductType>>(jsonValue.ToString());
								break;

							case DisplayNames.SHAPE_CODE:
								newSpec.Value = jsonValue.ToObject<ShapeCode>();
								break;

							case DisplayNames.FEET:
							case DisplayNames.DRAIN_HOLES:
								newSpec.Value = jsonValue.ToObject<bool>();
								break;

							case DisplayNames.FINISH:
								newSpec.Value = App.Data.Finishes.FirstOrDefault(f => f.id.value.ToString() == jsonValue.ToString());
								break;

							case DisplayNames.BURNER:
								newSpec.Value = App.Data.Burners.FirstOrDefault(b => b.id.value.ToString() == jsonValue.ToString());
								break;

							case DisplayNames.FUEL_TYPE:
								newSpec.Value = App.Data.FuelTypes.FirstOrDefault(ft => ft.id.value.ToString() == jsonValue.ToString());
								break;

							case DisplayNames.FILL:
								newSpec.Value = App.Data.Fills.FirstOrDefault(f => f.id.value.ToString() == jsonValue.ToString());
								break;

							case DisplayNames.PANEL:
								newSpec.Value = App.Data.Panels.FirstOrDefault(p => p.id.value.ToString() == jsonValue.ToString());
								break;

							case DisplayNames.MATERIAL:
								newSpec.Value = jsonValue.ToObject<BuildMaterial>();
								break;

							case DisplayNames.THICKNESS:
								newSpec.Value = jsonValue.ToObject<ThicknessEnum>();
								break;

							case DisplayNames.BOWL_SIZE:
							case DisplayNames.ORB_SIZE:
							case DisplayNames.VASE_SIZE:
							case DisplayNames.BULB_SIZE:
							case DisplayNames.FIRE_BASIN_SIZE:
							case DisplayNames.FIRE_BOWL_SIZE:
							case DisplayNames.FIRE_DISH_SIZE:
							case DisplayNames.FIRE_ROUND_SIZE:
							case DisplayNames.WATER_BOWL_SIZE:
							case DisplayNames.DISH_SIZE:
								newSpec.Value = jsonValue.ToObject<BowlSizeEnum>();
								break;

							case DisplayNames.ADD_ON_OPTIONS:
								List<AddOnOption> selectedAddOnOptions = new List<AddOnOption>();
								jsonValue.ToArray().ToList().ForEach(jsonADO =>
								{
									AddOnOption newADO = App.Data.AddOnOptions.FirstOrDefault(ado => ado.id == jsonADO["id"].ToString());
									newADO.quantity.value = jsonADO["quantity"].ToObject<double>();
									newADO.purchasePrice.value = jsonADO["purchasePrice"].ToObject<double>();
									newADO.costing = JsonConvert.DeserializeObject<Costing>(jsonADO["costing"].ToString());
									selectedAddOnOptions.Add(newADO);
								});
								newSpec.Value = selectedAddOnOptions;
								break;

							default:
								newSpec.Value = jsonValue.ToObject<double?>();
								break;
						}
						newSpec.ParentBuild = newBuild;
						sf.SetValue(newBuild, newSpec);
					}
				}));

				newBuild.costing = JsonConvert.DeserializeObject<Costing>(json["costing"].ToString());

				SKUTemplate matchingSKUTemplate = App.Data.SKULibrary.FirstOrDefault(t => t.ShapeCode == newBuild.ShapeCode);
				newBuild.readOnlySpecs.AddRange(matchingSKUTemplate.allowModification ? matchingSKUTemplate.ReadOnlySpecs : Constants.NO_MOD_SPECS);

				return newBuild;
			}
			
			public override void WriteJson(JsonWriter writer, object data, JsonSerializer serializer)
			{
				Build build = (Build)data;
				writer.WriteStartObject();
				Build.SpecFields.ForEach((Action<FieldInfo>)(sf =>
				{
					string specName = sf.Name;
					Spec<object> spec = ((Spec<object>)sf.GetValue(build));
					if (spec != null)
					{
						writer.WritePropertyName(specName);
						switch (spec.displayName)
						{
							case DisplayNames.SKU:
							case DisplayNames.TITLE:
								serializer.Serialize(writer, (object)spec.Value.ToString());
								break;

							case DisplayNames.MATERIAL:
								serializer.Serialize(writer, (int)build.Material);
								break;

							case DisplayNames.THICKNESS:
								serializer.Serialize(writer, (int)build.Thickness);
								break;

							case DisplayNames.FINISH:
								serializer.Serialize(writer, build.Finish.id.value);
								break;

							case DisplayNames.FINISH_TYPE:
								serializer.Serialize(writer, (int)build.FinishType);
								break;

							case DisplayNames.BURNER:
								serializer.Serialize(writer, build.Burner.id.value);
								break;

							case DisplayNames.FUEL_TYPE:
								serializer.Serialize(writer, build.FuelType.id.value);
								break;

							case DisplayNames.FILL:
								serializer.Serialize(writer, build.Fill.id.value);
								break;

							case DisplayNames.PANEL:
								serializer.Serialize(writer, build.Panel.id.value);
								break;

							case DisplayNames.BOWL_SIZE:
							case DisplayNames.ORB_SIZE:
							case DisplayNames.VASE_SIZE:
							case DisplayNames.BULB_SIZE:
							case DisplayNames.FIRE_BASIN_SIZE:
							case DisplayNames.FIRE_BOWL_SIZE:
							case DisplayNames.FIRE_DISH_SIZE:
							case DisplayNames.FIRE_ROUND_SIZE:
							case DisplayNames.WATER_BOWL_SIZE:
							case DisplayNames.DISH_SIZE:
								serializer.Serialize(writer, (int)build.BowlSize.EnumValue);
								break;

							case DisplayNames.ADD_ON_OPTIONS:
								writer.WriteStartArray();
								build.AddOnOptions.ForEach(ado =>
								{
									writer.WriteStartObject();
									writer.WritePropertyName("id");
									serializer.Serialize(writer, ado.id.value);
									writer.WritePropertyName("quantity");
									serializer.Serialize(writer, ado.quantity.value);
									writer.WritePropertyName("purchasePrice");
									serializer.Serialize(writer, ado.purchasePrice.value);
									AddCostingToJSON(ado.costing);
									writer.WriteEndObject();
								});
								writer.WriteEndArray();
								break;

							case DisplayNames.SHAPE_CODE:
							case DisplayNames.QUANTITY:
							case DisplayNames.CATEGORY:
							case DisplayNames.PRODUCT_TYPES:
							case DisplayNames.DRAIN_HOLES:
							case DisplayNames.FEET:
								serializer.Serialize(writer, (object)spec.Value);
								break;

							default:
								serializer.Serialize(writer, Convert.ToDouble((object)spec.Value));
								break;
						}
					}
				}));

				AddCostingToJSON(build.costing);

				writer.WriteEndObject();



				void AddCostingToJSON(Costing costing)
				{
					writer.WritePropertyName("costing");
					writer.WriteStartObject();
					Costing.BOMFields.ForEach(bf =>
					{
						BOMEntry bomEntry = ((BOMEntry)bf.GetValue(costing));
						if (bomEntry.Quantity == null && bomEntry.Cost == null) { return; }
						writer.WritePropertyName(bf.Name);
						writer.WriteStartObject();
						writer.WritePropertyName("quantity");
						serializer.Serialize(writer, bomEntry.Quantity);
						writer.WritePropertyName("cost");
						serializer.Serialize(writer, bomEntry.Cost);
						writer.WriteEndObject();
					});
					Costing.RoutingFields.ForEach(rf =>
					{
						RoutingEntry routingEntry = ((RoutingEntry)rf.GetValue(costing));
						if (routingEntry.Quantity == null && routingEntry.Time == null && routingEntry.Cost == null) { return; }
						writer.WritePropertyName(rf.Name);
						writer.WriteStartObject();
						writer.WritePropertyName("quantity");
						serializer.Serialize(writer, routingEntry.Quantity);
						writer.WritePropertyName("time");
						serializer.Serialize(writer, routingEntry.Time);
						writer.WritePropertyName("cost");
						serializer.Serialize(writer, routingEntry.Cost);
						writer.WriteEndObject();
					});
					writer.WriteEndObject();
				}
			}

			public override bool CanConvert(Type objectType)
			{
				return typeof(Build).IsAssignableFrom(objectType);
			}
		}
		/**/
	}
}
