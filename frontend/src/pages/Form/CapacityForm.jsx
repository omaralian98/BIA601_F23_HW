import React, { useEffect, useState } from "react";
import useFormData from "../../store";
import { Bounce, toast, ToastContainer } from "react-toastify";
import Input from "../../components/Input";
import { Link, useNavigate } from "react-router-dom";
import { mode } from "d3";

const CapacityForm = () => {
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
  const { formDataStorage, addTruckCapacities } = useFormData();
  const [formData, setFormData] = useState([]);
  let temp = [];

  for (let i = 0; i < formDataStorage.trucks_count; i++) {
    temp[i] = i;
  }

  const handleChange = (event, index) => {
    formData[index] = parseInt(event.target.value);
    setFormData(formData);
  };

  const isValid = formData.length == formDataStorage.trucks_count;

  return (
    <div className="flex justify-center p-6 gap-4 w-full h-full bg-slate-200 border border-gray-400 rounded-lg flex-col overflow-y-scroll">
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
        {temp.map((e, i) => (
          <div className="md:w-[45%] w-[90%]">
            <Input
              type="number"
              placeholder="Truck capacity"
              name={`Truck ${i + 1} capacity`}
              onChange={(e) => handleChange(e, i)}
            />
          </div>
        ))}
      </div>
      <div className="w-full flex gap-4 h-fit justify-around">
        <button
          onClick={() => {
            addTruckCapacities(formData);
            navigate(-1);
          }}
          className=" bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
          Previous
        </button>
        <Link
          onClick={() => {
            if (formData.length < formDataStorage.trucks_count) {
              notify(`All fields are required`);
              return;
            }
            addTruckCapacities(formData);
          }}
          to={
            isValid
              ? mode.link === "mode3"
                ? "/objects"
                : "/truck-details"
              : ""
          }
          className=" bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
          Next
        </Link>
      </div>
    </div>
  );
};

export default CapacityForm;
