import React, { useEffect } from "react";
import useFormData from "../../store";
import { usePost } from "../../Tools/APIs";
import { useNavigate } from "react-router-dom";

const FinalPage = () => {
  const navigate = useNavigate();
  const { handleSubmit, successfulPost } = usePost();
  const { formDataToSend, setFormDataToSend, mode, formDataStorage } =
    useFormData();

  useEffect(() => {
    setFormDataToSend({
      [mode.link === "mode1" || mode.link === "mode2"
        ? "capacity"
        : "capacities"]:
        mode.link === "mode1" || mode.link === "mode2"
          ? formDataStorage.capacity
          : formDataStorage.capacities,
      distances: formDataStorage.locations.distances,
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

  return (
    <div className="flex flex-col justify-center items-center text-center gap-5">
      <h1 className="text-xl text-gray-800 font-bold">
        You're finally finished
      </h1>
      <button
        onClick={() => {
          handleSubmit(
            `https://bia601api-001-site1.ltempurl.com/api/${mode.link}`,
            formDataToSend
          );
          if (successfulPost) navigate("/map/nodes");
        }}
        className="bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
        Submit
      </button>
    </div>
  );
};

export default FinalPage;
