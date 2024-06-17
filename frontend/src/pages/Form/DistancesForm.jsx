import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import Input from "../../components/Input";
import useFormData from "../../store";
import { Bounce, ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const DistancesForm = () => {
  const notify = (message) =>
    toast.error(`${message}`, {
      position: "top-center",
      autoClose: 2000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      progress: undefined,
      theme: "light",
      transition: Bounce,
    });

  const navigate = useNavigate();
  const [isEmpty, setIsEmpty] = useState(true);
  const [show, setShow] = useState(false);
  const [timeUnit, setTimeUnit] = useState("minutes");
  const [truckSpeed, setTruckSpeed] = useState(0);
  const [possibles, setPossibles] = useState([]);
  const { formDataStorage, addLoctaionsTime, mode } = useFormData();
  const locations = formDataStorage.locations.locations_name;
  const distances = [];
  console.log(mode);

  let counter = 0;

  useEffect(() => {
    for (let i = 0; i < locations?.length; i++) {
      possibles[i] = [];
      for (let j = 0; j < locations?.length; j++) {
        possibles[i].push({
          id: counter,
          start: locations[i],
          end: locations[j],
          distance: 0,
        });
        counter++;
      }
    }
    setPossibles(possibles);
  }, []);

  const handleUpdateTimeAfterSpeed = () => {
    setPossibles(
      possibles.map((e) =>
        e.map((element) =>
          element.start !== element.end
            ? {
                ...element,
                distance:
                  element["distance"] *
                  (show ? (truckSpeed ? truckSpeed : truckSpeed) : 70),
              }
            : { ...element }
        )
      )
    );
  };

  const handleTimeChange = (event, locatinId) => {
    for (let i = 0; i < possibles.length; i++) {
      for (let j = 0; j < possibles[i].length; j++) {
        if (possibles[i][j]["id"] === locatinId) {
          possibles[i][j]["distance"] = parseInt(event.target.value);
          console.log("a");
        } else continue;
      }
    }
  };

  const checkIsEmpty = () => {
    for (let i = 0; i < possibles.length; i++) {
      for (let j = 0; j < possibles[i].length; j++) {
        if (possibles[i][j]["start"] === possibles[i][j]["end"]) continue;
        if (!possibles[i][j]["distance"]) {
          notify("All fields are required");
          setIsEmpty(true);
          console.log("a");
          return false;
        }
      }
      setIsEmpty(false);
      return true;
    }
  };
  const handleAddDistances = () => {
    for (let i = 0; i < possibles.length; i++) {
      distances[i] = [];
      for (let j = 0; j < possibles[i].length; j++) {
        distances[i][j] =
          possibles[i][j]["distance"] *
          (timeUnit === "hours" ? 1 : 60) *
          (truckSpeed ? truckSpeed : 70);
      }
    }

    addLoctaionsTime(distances);
  };

  return (
    <div className="flex justify-center p-6 gap-4 w-full h-full bg-slate-200 border border-gray-400 rounded-lg flex-col">
      <ToastContainer
        position="top-center"
        autoClose={2000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="light"
        transition={Bounce}
      />
      <div className="w-[80%] mx-auto flex flex-1 justify-center gap-4 border border-gray-900 rounded-xl bg-gray-300 p-10 items-center flex-wrap overflow-y-auto">
        <div className="flex md:justify-around gap-3 w-full max-md:flex-col max-md:items-center">
          <div className="flex flex-col gap-3 items-center">
            <h1 className="text-xl text-gray-700 font-bold">
              Choose the time unit
            </h1>
            <div className="flex justify-center gap-4 w-full">
              <button
                onClick={() => setTimeUnit("hours")}
                className={`text-center font-semibold px-6 py-2 ${
                  timeUnit === "hours"
                    ? "bg-blue-500 text-white"
                    : "bg-slate-100"
                }  rounded-xl`}>
                Hours
              </button>
              <button
                onClick={() => setTimeUnit("minutes")}
                className={`text-center font-semibold px-6 py-2 ${
                  timeUnit === "minutes"
                    ? "bg-blue-500 text-white"
                    : "bg-slate-100"
                }  rounded-xl`}>
                Minutes
              </button>
            </div>
          </div>
          <div className="flex flex-col justify-center items-center">
            <h1 className="text-lg font-bold">
              The current truck speed is:{" "}
              {show ? (truckSpeed ? truckSpeed : 70) : 70}kmph
            </h1>
            <div className="flex gap-2">
              <label
                className="font-semibold"
                htmlFor="truck-speed">{`Change it (optional)`}</label>
              <input
                type="checkbox"
                onClick={() => {
                  setShow(!show);
                }}
              />
            </div>
            {show && (
              <Input
                type="number"
                placeholder="Change speed"
                value={truckSpeed}
                onChange={(e) => {
                  setTruckSpeed(e.target.value);
                }}
              />
            )}
          </div>
        </div>
        {possibles?.map((e) =>
          e?.map(
            (element) =>
              element.start !== element.end && (
                <div key={element.id} className="md:w-[45%] w-[80%]">
                  <Input
                    type="number"
                    placeholder="Enter time"
                    name={`from ${element.start} to ${element.end}`}
                    onChange={(event) => handleTimeChange(event, element?.id)}
                  />
                </div>
              )
          )
        )}
      </div>
      <div className="w-full flex justify-around gap-4">
        <button
          className=" bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300"
          onClick={() => navigate(-1)}>
          Previous
        </button>
        <Link
          to={
            !isEmpty
              ? mode.link !== "mode1" && mode.link !== "mode2"
                ? "/capacities"
                : "/objects"
              : ""
          }
          onClick={() => {
            if (!checkIsEmpty()) return;
            handleUpdateTimeAfterSpeed();
            handleAddDistances();
          }}
          className="bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
          Next
        </Link>
      </div>
    </div>
  );
};
export default DistancesForm;
