import React, { useEffect } from "react";
import useFormData from "../../store";
import { usePost } from "../../Tools/APIs";
import { useNavigate } from "react-router-dom";

const FinalPage = () => {
  const navigate = useNavigate();
  const { handleSubmit } = usePost();
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
      [(mode.link === "mode5" ||
        mode.link === "mode6" ||
        mode.link === "mode7") &&
      "indicesofpickinguppoints"]: formDataStorage.indicesofpickinguppoints,
      [(mode.link === "mode5" ||
        mode.link === "mode6" ||
        mode.link === "mode7") &&
      "indicesofdroppingoffpoints"]: formDataStorage.indicesofdroppingoffpoints,
      [(mode.link === "mode6" || mode.link === "mode7") && "pickUpPenalties"]:
        formDataStorage.pickUpPenalties,
      [(mode.link === "mode6" || mode.link === "mode7") && "dropOffPenalties"]:
        formDataStorage.dropOffPenalties,
    });
  }, []);

  return (
    <div className="flex flex-col justify-center items-center gap-5">
      <h1 className="text-xl text-gray-800 font-bold">
        you're finally finished
      </h1>
      <button
        onClick={() => {
          handleSubmit(
            `https://80af-94-47-157-18.ngrok-free.app/api/mode3`,
            formDataToSend
          );
          navigate("/");
        }}
        className="bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
        Submit
      </button>
    </div>
  );
};

export default FinalPage;
