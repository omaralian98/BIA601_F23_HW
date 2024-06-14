import React, { useEffect, useState } from "react";
import useFormData from "../../store";
import Input from "../../components/Input";
import { Link, useNavigate } from "react-router-dom";
import { Bounce, ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const LocationsForm = () => {
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
  const { formDataStorage, addLocations } = useFormData();
  const [location, setLocation] = useState("");
  const [formData, setFormData] = useState([]);
  const locations_count = formDataStorage.locations_count;

  useEffect(() => {
    setFormData(formDataStorage?.locations?.locations_name || []);
  }, []);

  const handleChange = (e) => {
    setLocation(e.target.value);
  };

  const handleAddLocation = () => {
    if (!location) {
      notify("The location field is required");
      return;
    }
    for (let i = 0; i < formData.length; i++) {
      if (formData[i] === location) {
        notify("This location is already exist");
        return;
      }
    }
    if (formData.length >= locations_count) {
      notify("You reached the maximum number of locations");
      return;
    }
    setLocation("");
    setFormData([...formData, location]);
  };

  const handleKeyDown = (event) => {
    if (event.key === "Enter") {
      handleAddLocation();
    }
  };

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
        <div className="md:w-[45%] w-[100%]">
          <Input
            type="text"
            placeholder="Location name"
            name="location"
            onChange={handleChange}
            onKeyDown={handleKeyDown}
            value={location}
          />
        </div>
        <div className="w-full flex justify-center gap-3 flex-wrap">
          {formData.length > 0 &&
            formData.map((e) => (
              <div
                key={e}
                className="relative rounded-xl bg-gray-800 p-4 flex justify-center items-center">
                <p className="text-white font-semibold">{e}</p>
                <span
                  onClick={() =>
                    setFormData(formData.filter((element) => element !== e))
                  }
                  className="absolute text-gray-200 -top-1 -left-1 rounded-full w-5 h-5 cursor-pointer flex justify-center items-center bg-red-500 hover:bg-red-400 font-semibold">
                  x
                </span>
              </div>
            ))}
        </div>
        <div className="w-full flex justify-end">
          <button
            onClick={handleAddLocation}
            className="bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
            Add location
          </button>
        </div>
      </div>
      <div className="w-full flex gap-4 h-fit justify-around">
        <button
          className=" bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300"
          onClick={() => navigate(-1)}>
          Previous
        </button>
        <Link
          onClick={() => {
            if (formData.length < locations_count) {
              notify(
                `Locations number are less than ${formDataStorage.locations_count}`
              );
              return;
            }
            addLocations(formData);
          }}
          to={formData.length == locations_count && "/page-4"}
          className=" bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
          Next
        </Link>
      </div>
    </div>
  );
};

export default LocationsForm;
