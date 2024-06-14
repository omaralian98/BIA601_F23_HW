import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import useFormData from "../../store";
import Input from "../../components/Input";
import { Bounce, ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const Page1 = () => {
  const notify = () =>
    toast.error(`All feilds are required`, {
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
  const { formDataStorage, setFormDataStorage, mode } = useFormData();

  const [formData, setFormData] = useState({
    [mode.link === "mode1" || mode.link === "mode2"
      ? "capacity"
      : "trucks_count"]: 0,
    objects_count: 0,
    locations_count: 0,
  });

  useEffect(() => {
    setFormData({
      [mode.link === "mode1" || mode.link === "mode2"
        ? "capacity"
        : "trucks_count"]:
        mode.link === "mode1" || mode.link === "mode2"
          ? formDataStorage.capacity
          : formDataStorage.trucks_count,
      objects_count: formDataStorage.objects_count,
      locations_count: formDataStorage.locations_count,
    });
  }, []);

  const checkIsEmpty = () => {
    for (const [key, value] of Object.entries(formData)) {
      if (!value) {
        console.log(key);
        return true;
      }
    }
    return false;
  };

  return (
    <div className="flex justify-center items-center p-6 gap-4 w-full h-full bg-slate-200 border border-gray-400 rounded-lg flex-wrap">
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
      <div className="flex flex-1 justify-center gap-3 border border-gray-900 rounded-xl bg-gray-300 p-10 items-center flex-wrap overflow-y-auto">
        {mode.link === "mode1" || mode.link === "mode2" ? (
          <div className="md:w-[45%] w-[90%]">
            <Input
              type="number"
              placeholder="Truck capacity"
              name="capacity"
              value={formData["capacity"]}
              onChange={(e) =>
                setFormData({ ...formData, capacity: e.target.value })
              }
            />
          </div>
        ) : (
          <div className="md:w-[45%] w-[90%]">
            <Input
              type="number"
              placeholder="Trucks count"
              name="trucks_count"
              value={formData["trucks_count"]}
              onChange={(e) =>
                setFormData({ ...formData, trucks_count: e.target.value })
              }
            />
          </div>
        )}
        <div className="md:w-[45%] w-[90%]">
          <Input
            type="number"
            placeholder="Objects count"
            name="objects_count"
            value={formData["objects_count"]}
            onChange={(e) =>
              setFormData({ ...formData, objects_count: e.target.value })
            }
          />
        </div>
        <div className="md:w-[45%] w-[90%]">
          <Input
            type="number"
            placeholder="Locations count"
            name="locations_count"
            value={formData["locations_count"]}
            onChange={(e) =>
              setFormData({ ...formData, locations_count: e.target.value })
            }
          />
        </div>
      </div>
      <div className="w-full flex gap-4 justify-around">
        <Link
          to="map/nodes"
          className="text-center text-lg bg-blue-600 px-8 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
          Go to Map
        </Link>
        <Link
          onClick={() => {
            if (checkIsEmpty()) {
              notify();
              return;
            } else setFormDataStorage(formData);
          }}
          to={
            !checkIsEmpty()
              ? mode.link === "mode1" || mode.link === "mode2"
                ? "/page-2"
                : "/capacities"
              : ""
          }
          className="text-center text-lg bg-blue-600 px-8 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-gray-300">
          Next
        </Link>
      </div>
    </div>
  );
};

export default Page1;
