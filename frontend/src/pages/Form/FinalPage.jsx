import React, { useEffect } from "react";
import useFormData from "../../store";
import { usePost } from "../../Tools/APIs";
import { useNavigate } from "react-router-dom";

const FinalPage = () => {
  const navigate = useNavigate();
  const { handleSubmit, successfulPost } = usePost();
  const { formDataToSend, setFormDataToSend, mode, formDataStorage } =
    useFormData();
  const possibles = formDataStorage.locations.distances;
  let distances = [];
  for (let i = 0; i < possibles.length; i++) {
    distances[i] = [];
    for (let j = 0; j < possibles[i].length; j++) {
      if (i === j) distances[i][j] = 0;
      else distances[i][j] = possibles[i][j]["distance"];
    }
  }

  useEffect(() => {
    setFormDataToSend({
      [mode.link === "mode1" || mode.link === "mode2"
        ? "capacity"
        : "capacities"]:
        mode.link === "mode1" || mode.link === "mode2"
          ? formDataStorage.capacity
          : formDataStorage.capacities,
      distances,
      values: formDataStorage.objects.values,
      weights: formDataStorage.objects.weights,
      [mode.link !== "mode1" &&
      mode.link !== "mode2" &&
      mode.link !== "mode3" &&
      "indicesofstartingpointes"]: formDataStorage.indicesofstartingpointes,
      [mode.link !== "mode1" &&
      mode.link !== "mode2" &&
      mode.link !== "mode3" &&
      "indicesofendingpointes"]: formDataStorage.indicesofendingpointes,
      [(mode.link === "mode5" || mode.link === "mode6") &&
      "indicesofpickinguppoints"]: formDataStorage.indicesofpickinguppoints,
      [(mode.link === "mode5" || mode.link === "mode6") &&
      "indicesofdroppingoffpoints"]: formDataStorage.indicesofdroppingoffpoints,
      [mode.link === "mode6" && "pickUpPenalties"]:
        formDataStorage.pickUpPenalties,
      [mode.link === "mode6" && "dropOffPenalties"]:
        formDataStorage.dropOffPenalties,
    });
  }, []);

  useEffect(() => {
    if (successfulPost) navigate("/map/nodes");
  }, [successfulPost]);

  return (
    <div className="flex flex-col justify-center items-center text-center gap-5">
      <h1 className="text-xl text-gray-800 font-bold">
        You're finally finished
      </h1>
      <button
        onClick={() => {
          handleSubmit(mode.link, formDataToSend);
        }}
        className="bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
        Submit
      </button>
    </div>
  );
};

export default FinalPage;
