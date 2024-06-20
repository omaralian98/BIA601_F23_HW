import React, { useState } from "react";
import useFormData from "../../store";
import { Link } from "react-router-dom";

const Settings = () => {
  const { changeSettings, formDataToSend } = useFormData();
  const [formData, setFormData] = useState({
    algorithmForTSP: 4,
    algorithmForKnapsack: 4,
  });
  console.log(formDataToSend);

  return (
    <div className="flex justify-center p-6 gap-10 w-full h-full bg-slate-200 border border-gray-400 rounded-lg flex-wrap overflow-y-auto">
      <h1 className="text-6xl max-sm:text-4xl text-gray-700 font-bold mx-auto w-full text-center">
        Settings
      </h1>
      <div className="w-full flex justify-end items-center flex-warp">
        <Link
          to="advanced"
          className="px-6 py-3 bg-blue-500 hover:bg-blue-400 font-semibold text-xl rounded-xl text-white">
          Advanced Settings
        </Link>
      </div>
      <div className="w-2/5 max-md:w-[90%] mx-auto bg-gray-300 border border-gray-900 rounded-xl p-10 flex flex-col gap-5 justify-between">
        <h2 className="text-2xl text-gray-800 font-bold text-start">
          Choose algorithm for TSP:
        </h2>
        <div className="flex gap-3 justify-between w-full">
          <label
            htmlFor="Brute_Force"
            className="font-semibold text-gray-700 text-xl">
            Brute Force
          </label>
          <input
            type="checkbox"
            id="Brute_Force"
            value={0}
            onChange={(e) =>
              setFormData((prev) => ({
                ...prev,
                algorithmForTSP: parseInt(e.target.value),
              }))
            }
            checked={formData.algorithmForTSP == 0}
          />
        </div>
        <div className="flex gap-3 justify-between w-full">
          <label
            htmlFor="Greedy"
            className="font-semibold text-gray-700 text-xl">
            Greedy
          </label>
          <input
            type="checkbox"
            id="Greedy"
            value={1}
            onChange={(e) =>
              setFormData((prev) => ({
                ...prev,
                algorithmForTSP: parseInt(e.target.value),
              }))
            }
            checked={formData.algorithmForTSP == 1}
          />
        </div>
        <div className="flex gap-3 justify-between w-full">
          <label
            htmlFor="Branch_And_Bound"
            className="font-semibold text-gray-700 text-xl">
            Branch and Bound
          </label>
          <input
            type="checkbox"
            id="Branch_And_Bound"
            value={2}
            onChange={(e) =>
              setFormData((prev) => ({
                ...prev,
                algorithmForTSP: parseInt(e.target.value),
              }))
            }
            checked={formData.algorithmForTSP == 2}
          />
        </div>
        <div className="flex gap-3 justify-between w-full">
          <label
            htmlFor="Dynamic"
            className="font-semibold text-gray-700 text-xl">
            Dynamic
          </label>
          <input
            type="checkbox"
            id="Dynamic"
            value={3}
            onChange={(e) =>
              setFormData((prev) => ({
                ...prev,
                algorithmForTSP: parseInt(e.target.value),
              }))
            }
            checked={formData.algorithmForTSP == 3}
          />
        </div>
        <div className="flex gap-3 justify-between w-full">
          <label
            htmlFor="Genetic"
            className="font-semibold text-gray-700 text-xl">
            Genetic
          </label>
          <input
            type="checkbox"
            id="Genetic"
            value={4}
            onChange={(e) =>
              setFormData((prev) => ({
                ...prev,
                algorithmForTSP: parseInt(e.target.value),
              }))
            }
            checked={formData.algorithmForTSP == 4}
          />
        </div>
      </div>

      <div className="w-2/5 max-md:w-[90%] mx-auto bg-gray-300 border border-gray-900 rounded-xl p-10 flex flex-col gap-5 justify-between">
        <h2 className="text-2xl text-gray-800 font-bold text-start">
          Choose algorithm for Knapsack:
        </h2>
        <div className="flex gap-3 justify-between w-full">
          <label
            htmlFor="Brute_Force"
            className="font-semibold text-gray-700 text-xl">
            Brute Force
          </label>
          <input
            type="checkbox"
            id="Brute_Force"
            value={0}
            onChange={(e) =>
              setFormData((prev) => ({
                ...prev,
                algorithmForKnapsack: parseInt(e.target.value),
              }))
            }
            checked={formData.algorithmForKnapsack == 0}
          />
        </div>
        <div className="flex gap-3 justify-between w-full">
          <label
            htmlFor="Greedy"
            className="font-semibold text-gray-700 text-xl">
            Greedy
          </label>
          <input
            type="checkbox"
            id="Greedy"
            value={1}
            onChange={(e) =>
              setFormData((prev) => ({
                ...prev,
                algorithmForKnapsack: parseInt(e.target.value),
              }))
            }
            checked={formData.algorithmForKnapsack == 1}
          />
        </div>
        <div className="flex gap-3 justify-between w-full">
          <label
            htmlFor="Branch_And_Bound"
            className="font-semibold text-gray-700 text-xl">
            Branch and Bound
          </label>
          <input
            type="checkbox"
            id="Branch_And_Bound"
            value={2}
            onChange={(e) =>
              setFormData((prev) => ({
                ...prev,
                algorithmForKnapsack: parseInt(e.target.value),
              }))
            }
            checked={formData.algorithmForKnapsack == 2}
          />
        </div>
        <div className="flex gap-3 justify-between w-full">
          <label
            htmlFor="Dynamic"
            className="font-semibold text-gray-700 text-xl">
            Dynamic
          </label>
          <input
            type="checkbox"
            id="Dynamic"
            value={3}
            onChange={(e) =>
              setFormData((prev) => ({
                ...prev,
                algorithmForKnapsack: parseInt(e.target.value),
              }))
            }
            checked={formData.algorithmForKnapsack == 3}
          />
        </div>
        <div className="flex gap-3 justify-between w-full">
          <label
            htmlFor="Genetic"
            className="font-semibold text-gray-700 text-xl">
            Genetic
          </label>
          <input
            type="checkbox"
            id="Genetic"
            value={4}
            onChange={(e) =>
              setFormData((prev) => ({
                ...prev,
                algorithmForKnapsack: parseInt(e.target.value),
              }))
            }
            checked={formData.algorithmForKnapsack == 4}
          />
        </div>
      </div>
      <div className="flex justify-center items-center w-full">
        <button
          onClick={() => changeSettings(formData)}
          className="text-white font-semibold bg-blue-500 hover:bg-blue-400 px-10 py-4 rounded-xl text-2xl">
          Save
        </button>
      </div>
    </div>
  );
};

export default Settings;
