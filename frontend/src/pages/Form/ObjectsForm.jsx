import React, { useEffect, useState } from "react";
import useFormData from "../../store";
import { Link, useNavigate } from "react-router-dom";
import Input from "../../components/Input";
import { Bounce, ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const ObjectForm = () => {
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
  const { formDataStorage, addObjects, mode } = useFormData();
  const [objectSpecs, setObjectSpecs] = useState({
    name: "",
    value: 0,
    weight: 0,
  });
  const [formData, setFormData] = useState({
    names: [],
    values: [],
    weights: [],
  });
  const objectCount = formDataStorage.objects_count;

  useEffect(() => {
    setFormData({
      names: formDataStorage?.objects?.names || [],
      values: formDataStorage?.objects?.values || [],
      weights: formDataStorage?.objects?.weights || [],
    });
  }, []);

  const handleDeleteObject = (name) => {
    let index = formData.names.indexOf(name);
    setFormData({
      names: formData.names.filter((e, i) => i !== index),
      values: formData.values.filter((e, i) => i !== index),
      weights: formData.weights.filter((e, i) => i !== index),
    });
  };

  const handleAddObject = () => {
    if (formData.names.length >= formDataStorage.objects_count) {
      notify("You reached the maximum number of objects");
      return;
    }
    for (const [key, value] of Object.entries(objectSpecs)) {
      if (!value) {
        notify("All fields are required");
        return;
      }
      if (key === "name") {
        if (formData.names.includes(value)) {
          notify("This object is already exists");
          return;
        }
      }
    }
    setFormData({
      names: [...formData.names, objectSpecs.name],
      values: [...formData.values, objectSpecs.value],
      weights: [...formData.weights, objectSpecs.weight],
    });
    setObjectSpecs({
      name: "",
      value: "",
      weight: "",
    });
  };

  const handleKeyDown = (event) => {
    if (event.key === "Enter") {
      handleAddObject();
    }
  };

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
        <div className="md:w-[45%] w-[90%]">
          <Input
            type="text"
            placeholder="Object name"
            name="name"
            value={objectSpecs["name"]}
            onChange={(e) =>
              setObjectSpecs((prev) => ({ ...prev, name: e.target.value }))
            }
            onKeyDown={handleKeyDown}
          />
        </div>
        <div className="md:w-[45%] w-[90%]">
          <Input
            type="number"
            placeholder="Object value"
            name="value in Dollars($)"
            value={objectSpecs["value"]}
            onChange={(e) =>
              setObjectSpecs((prev) => ({ ...prev, value: e.target.value }))
            }
            onKeyDown={handleKeyDown}
          />
        </div>
        <div className="md:w-[45%] w-[90%]">
          <Input
            type="number"
            placeholder="Object weight"
            name="weight in kilograms(kg)"
            value={objectSpecs["weight"]}
            onChange={(e) =>
              setObjectSpecs((prev) => ({ ...prev, weight: e.target.value }))
            }
            onKeyDown={handleKeyDown}
          />
        </div>
        <div className="w-full flex justify-center gap-3 flex-wrap">
          {formData?.names?.length > 0 &&
            formData?.names.map((e) => (
              <div
                key={e}
                className="relative rounded-xl bg-gray-800 p-4 flex justify-center items-center">
                <p className="text-white font-semibold">{e}</p>
                <span
                  onClick={() => handleDeleteObject(e)}
                  className="absolute text-gray-200 -top-1 -left-1 rounded-full w-5 h-5 cursor-pointer flex justify-center items-center bg-red-500 hover:bg-red-400 font-semibold">
                  x
                </span>
              </div>
            ))}
        </div>
        <div className="flex justify-end w-full">
          <button
            onClick={handleAddObject}
            className="bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
            Add object
          </button>
        </div>
      </div>
      <div className="w-full flex gap-4 h-fit justify-around flex-wrap">
        <button
          onClick={() => {
            addObjects(formData);
            navigate(-1);
          }}
          className=" bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-white">
          Previous
        </button>
        <Link
          onClick={() => {
            if (formData.names.length < formDataStorage.objects_count) {
              notify(
                `Objects count are less than ${formDataStorage.objects_count}`
              );
              return;
            }
            addObjects(formData);
          }}
          to={
            formData.names.length == objectCount
              ? mode.link === "mode5" ||
                mode.link === "mode6"
                ? "/objects-details"
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

export default ObjectForm;
