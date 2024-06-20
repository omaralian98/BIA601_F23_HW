import React, { useState } from "react";
import useFormData from "../../store";
import Input from "../../components/Input";

const AdvancedSettings = () => {
  const { changeSettings } = useFormData();
  const [formData, setFormData] = useState({});

  const handleChange = (e, name, key) => {
    setFormData({
      ...formData,
      [key]: {
        ...(formData[key] ? formData[key] : {}),
        [name]: parseInt(e.target.value),
      },
    });
  };

  return (
    <div className="flex justify-center p-6 gap-10 w-full h-full bg-slate-200 border border-gray-400 rounded-lg flex-wrap overflow-y-auto">
      <h1 className="text-6xl max-sm:text-4xl text-gray-700 font-bold mx-auto w-full text-center">
        Advanced Settings
      </h1>
      <div className="flex justify-center gap-10 w-full flex-wrap">
        <div className="w-2/5 max-md:w-[90%] mx-auto flex flex-col justify-between gap-4 bg-gray-300 p-10 rounded-xl">
          <h2 className="text-gray-800 font-bold text-2xl">
            Settings for Genetic TSP
          </h2>
          <Input
            type="number"
            name="Max iterations"
            placeholder="Max iterations"
            value={formData?.settingsForGeneticTSP?.maxIterations || 0}
            onChange={(e) =>
              handleChange(e, "maxIterations", "settingsForGeneticTSP")
            }
          />
          <Input
            type="number"
            name="Population size"
            placeholder="Population size"
            value={formData?.settingsForGeneticTSP?.populationSize || 0}
            onChange={(e) =>
              handleChange(e, "populationSize", "settingsForGeneticTSP")
            }
          />
          <Input
            type="number"
            name="Mutation probability"
            placeholder="Mutation probability"
            value={formData?.settingsForGeneticTSP?.mutationProbability || 0}
            onChange={(e) =>
              handleChange(e, "mutationProbability", "settingsForGeneticTSP")
            }
          />
          <Input
            type="number"
            name="Eliterate"
            placeholder="Eliterate"
            value={formData?.settingsForGeneticTSP?.eliterate || 0}
            onChange={(e) =>
              handleChange(e, "eliterate", "settingsForGeneticTSP")
            }
          />
          <Input
            type="number"
            name="Number of cross-over points"
            placeholder="Number of cross-over points"
            value={
              formData?.settingsForGeneticTSP?.numberOfCrossOverPoints || 0
            }
            onChange={(e) =>
              handleChange(
                e,
                "numberOfCrossOverPoints",
                "settingsForGeneticTSP"
              )
            }
          />
          <div className="flex justify-around gap-3 bg-slate-50 p-2 rounded-xl">
            <label htmlFor="fast" className="text-gray-900 font-semibold">
              Fast
            </label>
            <input
              type="checkbox"
              id="fast"
              name="fast"
              value={formData?.settingsForGeneticTSP?.fast || false}
              onChange={(e) =>
                setFormData((prev) => ({
                  ...prev,
                  settingsForGeneticTSP: {
                    ...prev.settingsForGeneticTSP,
                    fast: e.target.checked,
                  },
                }))
              }
            />
          </div>
        </div>
        <div className="w-2/5 max-md:w-[90%] mx-auto flex flex-col justify-between gap-4 bg-gray-300 p-10 rounded-xl">
          <h2 className="text-gray-800 font-bold text-2xl">
            Settings for Knapsack
          </h2>
          <Input
            type="number"
            name="Max iterations"
            placeholder="Max iterations"
            value={formData?.settingsForGeneticKnapsack?.maxIterations || 0}
            onChange={(e) =>
              handleChange(e, "maxIterations", "settingsForGeneticKnapsack")
            }
          />
          <Input
            type="number"
            name="Population size"
            placeholder="Population size"
            value={formData?.settingsForGeneticKnapsack?.populationSize || 0}
            onChange={(e) =>
              handleChange(e, "populationSize", "settingsForGeneticKnapsack")
            }
          />
          <Input
            type="number"
            name="Mutation probability"
            placeholder="Mutation probability"
            value={
              formData?.settingsForGeneticKnapsack?.mutationProbability || 0
            }
            onChange={(e) =>
              handleChange(
                e,
                "mutationProbability",
                "settingsForGeneticKnapsack"
              )
            }
          />
          <Input
            type="number"
            name="Eliterate"
            placeholder="Eliterate"
            value={formData?.settingsForGeneticKnapsack?.eliterate || 0}
            onChange={(e) =>
              handleChange(e, "eliterate", "settingsForGeneticKnapsack")
            }
          />
          <Input
            type="number"
            name="Number of cross-over points"
            placeholder="Number of cross-over points"
            value={
              formData?.settingsForGeneticKnapsack?.numberOfCrossOverPoints || 0
            }
            onChange={(e) =>
              handleChange(
                e,
                "numberOfCrossOverPoints",
                "settingsForGeneticKnapsack"
              )
            }
          />
          <div className="flex justify-around gap-3 bg-slate-50 p-2 rounded-xl">
            <label htmlFor="fast" className="text-gray-900 font-semibold">
              Fast
            </label>
            <input
              type="checkbox"
              id="fast"
              name="fast"
              value={formData?.settingsForGeneticKnapsack?.fast || false}
              onChange={(e) =>
                setFormData((prev) => ({
                  ...prev,
                  settingsForGeneticKnapsack: {
                    ...prev.settingsForGeneticKnapsack,
                    fast: e.target.checked,
                  },
                }))
              }
            />
          </div>
        </div>
      </div>
      <div className="flex justify-center items-center">
        <button
          onClick={() => changeSettings(formData)}
          className="text-white font-semibold bg-blue-500 hover:bg-blue-400 px-10 py-4 rounded-xl text-2xl">
          Save
        </button>
      </div>
    </div>
  );
};

export default AdvancedSettings;
