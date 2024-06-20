import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Bounce, toast, ToastContainer } from "react-toastify";
import useFormData from "../../store";
import Input from "../../components/Input";

const PickUpDropOff = () => {
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

  const { formDataStorage, addPickUpDropOffTime } = useFormData();
  const [formData, setFormData] = useState({
    pickUpPenalties: [],
    dropOffPenalties: [],
  });
  let temp = [];

  for (let i = 0; i < formDataStorage.objects_count; i++) {
    temp[i] = i;
  }

  const handleChange = (e, i, key) => {
    formData[key][i] = e.target.value;
    setFormData(formData);
  };

  let isEmpty =
    formData.dropOffPenalties.length === formData.pickUpPenalties.length &&
    formData.pickUpPenalties.length === parseInt(formDataStorage.objects_count);

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
        {temp.map((e, i) => (
          <div className="w-full flex gap-3 flex-wrap justify-between">
            <div className="md:w-[45%] w-[100%]">
              <Input
                type="number"
                placeholder="Picking up time"
                name={`Item ${i + 1}`}
                onChange={(e) => {
                  handleChange(e, i, "pickUpPenalties");
                }}
              />
            </div>
            <div className="md:w-[45%] w-[100%]">
              <Input
                type="number"
                placeholder="Droping off time"
                name={`Item ${i + 1}`}
                onChange={(e) => {
                  handleChange(e, i, "dropOffPenalties");
                }}
              />
            </div>
          </div>
        ))}
      </div>
      <div className="w-full flex gap-4 h-fit justify-around flex-warp">
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
            addPickUpDropOffTime(formData);
          }}
          to={isEmpty && "/final"}
          className=" bg-blue-600 px-4 py-3 hover:bg-blue-400 cursor-pointer rounded-lg font-bold text-white">
          Next
        </Link>
      </div>
    </div>
  );
};

export default PickUpDropOff;
