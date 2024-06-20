import axios from "axios";
import { useEffect, useState } from "react";
import useFormData from "../../store";
import base64 from "base-64";

const baseURL = process.env.REACT_APP_API_URL
  ? process.env.REACT_APP_API_URL
  : "https://bia601api-001-site1.ltempurl.com";

const username = "11182471";
const password = "60-dayfreetrial";

// Encode the credentials in Base64 using base-64 library
const token = base64.encode(`${username}:${password}`);

export const useGet = (mode) => {
  const [dataGet, setDataGet] = useState({});

  useEffect(() => {
    axios
      .get(`${baseURL}/api/${mode}`, {
        headers: {
          Authorization: `Basic ${token}`,
        },
      })
      .then((req) => setDataGet(req.data))
      .catch((error) => {
        console.log(error.message);
      });
  }, [mode, baseURL]);
  return { dataGet };
};

export const usePost = () => {
  const { setResponse } = useFormData();
  const [successfulPost, setSuccessfulPost] = useState(false);

  const handleSubmit = (mode, postData) => {
    setSuccessfulPost(false);
    axios
      .post(`${baseURL}/api/${mode}`, postData, {
        headers: {
          Authorization: `Basic ${token}`,
          "Content-Type": "application/json",
          "Access-Control-Allow-Origin": "*",
          "Access-Control-Allow-Headers": "*",
          "Control-Allow-Credentials": true,
        },
      })
      .then((response) => {
        if (response.data) {
          setResponse(response.data);
          setSuccessfulPost(true);
        }
      })
      .catch((error) => {
        console.error("Error posting data:", error.message);
      });
  };

  return { handleSubmit, successfulPost };
};
