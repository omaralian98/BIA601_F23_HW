import React, { useState } from "react";
import { Bounce, toast, ToastContainer } from "react-toastify";
import Input from "../../components/Input";
import { Link, useNavigate } from "react-router-dom";
import useFormData from "../../store";

const StartEndObjects = () => {
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

  const { formDataStorage, addStartEndIndexesObjects, mode } = useFormData();
  const [formData, setFormData] = useState({
    indicesofpickinguppoints: [],
    indicesofdroppingoffpoints: [],
  });
  let fields = [];
  const [temp, setTemp] = useState({
    indicesofpickinguppoints: [],
    indicesofdroppingoffpoints: [],
  });

  for (let i = 0; i < formDataStorage.objects_count; i++) {
    fields[i] = i;
  }

  const handleChange = (e, i, key) => {
    temp[key][i] = e.target.value;
    setTemp(temp);
  };

  const handleCheckLocations = () => {
    for (const [key, value] of Object.entries(temp)) {
      for (let i = 0; i < value.length; i++) {
        if (formDataStorage.locations.locations_name.indexOf(value[i]) === -1) {
          return value[i];
        }
      }
    }
    return true;
  };

  const missedValue = handleCheckLocations();

  let isEmpty =
    temp.indicesofdroppingoffpoints.length ===
      temp.indicesofpickinguppoints.length &&
    temp.indicesofpickinguppoints.length ===
      parseInt(formDataStorage.objects_count);

  return (
    <div className="flex justify-center p-6 gap-4 w-full h-full bg-slate-200 border border-gray-400 rounded-lg flex-col overflow-y-auto">
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
      <div className="w-[80%] mx-auto flex flex-1 justify-center gap-3 border border-gray-900 rounded-xl bg-gray-300 p-10 items-center flex-wrap overflow-y-auto">
        {fields.map((e, i) => (
          <div className="w-full flex gap-3 flex-wrap justify-between">
            <div className="md:w-[45%] w-[100%]">
              <Input
                type="text"
                placeholder="Starting location"
                name={`${formDataStorage.objects.names[i]}`}
                onChange={(e) => {
                  handleChange(e, i, "indicesofpickinguppoints");
                }}
              />
            </div>
            <div className="md:w-[45%] w-[100%]">
              <Input
                type="text"
                placeholder="Ending location"
                name={`${formDataStorage.objects.names[i]}`}
                onChange={(e) => {
                  handleChange(e, i, "indicesofdroppingoffpoints");
                }}
              />
            </div>
          </div>
        ))}
      </div>
      <div className="w-full flex gap-4 h-fit justify-around flex-wrap">
        <button
          className=" bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-white"
          onClick={() => navigate(-1)}>
          Previous
        </button>
        <Link
          onClick={() => {
            if (!isEmpty) {
              notify(`All fields are required`);
              return;
            }
            if (missedValue !== true) {
              notify(`${missedValue} does not exist in locations`);
            } else {
              setFormData(temp);
              addStartEndIndexesObjects(formData);
            }
          }}
          to={
            isEmpty && missedValue === true
              ? mode.link === "mode6"
                ? "/objects-pick-drop"
                : "/final"
              : ""
          }
          className=" bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-white">
          Next
        </Link>
      </div>
    </div>
  );
};

export default StartEndObjects;
